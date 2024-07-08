using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;
using System.Globalization;
namespace EMS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        public UserService(MyDbContext context, IConfiguration configuration, ILogger<UserService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                return user;
            }
            else return null;

        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                return user;
            }
            else return null;
        }

        public async Task<List<User>> GetByJobPositionAsync(string jobPosition)
        {
            var user = await _context.Users.Where(u => u.JobPosition == jobPosition).ToListAsync();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                return user;
            }
            else return null;
        }

        public async Task<User> CreateAsync(UserDto UserDto)
        {
            try
            {
                bool usernameExists = await _context.Users.AnyAsync(u => u.Username == UserDto.Username);
                bool emailExists = await _context.Users.AnyAsync(u => u.Email == UserDto.Email);
                if (!usernameExists && !emailExists)
                {
                    var user = new User
                    {
                        Username = UserDto.Username,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(UserDto.Password),
                        Email = UserDto.Email,
                        FullName = UserDto.FullName,
                        JobPosition = UserDto.JobPosition,
                        Role = UserDto.Role
                    };
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return user;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public Task<bool> DeleteAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserAsync(UserDto UserDto)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == UserDto.Username);
                if (user != null)
                {
                    user.Username = UserDto.Username;
                    user.PasswordHash = UserDto.Password;
                    user.FullName = UserDto.FullName;
                    user.Email = UserDto.Email;
                    user.JobPosition = UserDto.JobPosition;
                    user.Role = UserDto.Role;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<APIResponse> LoginAsync(Login login)
        {
            try
            {
                // Buoc 1. Kiểm tra username và password
                if (login == null || string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                {
                    return new APIResponse
                    {
                        Message = "Yêu cầu đăng nhập không hợp lệ",
                        Token = ""
                    };
                }

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == login.Username);
                //Neu username dung
                if (user != null)
                {
                    //Kiem tra trang thai tai khoan
                    if(user.isLocked)
                    {
                        return new APIResponse
                        {
                            Message = "Tài khoản của bạn bị khóa. ",
                            Token = ""
                        };
                    }
                    //kiem tra password
                    bool checkpass = BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
                    if (!checkpass)
                    {
                        user.LoginAttempts += 1;
                        //Dang nhap sai qua 5 lan
                        if(user.LoginAttempts >= 5)
                        {
                            user.isLocked = true;
                            user.LoginAttempts  = 0;
                            _context.Update(user);
                            await _context.SaveChangesAsync();
                            return new APIResponse
                            {
                                Message = "Tài khoản của bạn bị khóa do đăng nhập sai 5 lần.",
                                Token = ""
                            };
                            
                        }

                        _context.Update(user);
                        await _context.SaveChangesAsync();

                        return new APIResponse
                        {
                            Message = "Đăng nhập sai mật khẩu.",
                            Token = ""
                        };
                    }

                    //b2. đăng nhập thành công.
                    user.LoginAttempts = 0;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    //Kiem tra token co duoc cap hay chua (user chưa log out). neu chua thi cap moi.
                    var existingToken = await _context.Tokens.FirstOrDefaultAsync(t =>
                        t.UserId == user.Id && !t.IsRevoke);
                    //nếu existingToken != null có nghĩa token đang active,thu hồi token cũ và cấp mới
                    if (existingToken != null)
                    {
                        existingToken.IsRevoke = true;
                        _context.Tokens.Update(existingToken);
                    }
                   
                    var token = GenerateToken(user);

                    var createdAt = DateTime.Now; // Thời gian hiện tại
                    var expiration = createdAt.AddDays(1);

                    // Định dạng thời gian hiện tại và thời gian hết hạn thành chuỗi
                    string createdAtString = createdAt.ToString("yyyy-MM-dd HH:mm:ss");
                    string expirationString = expiration.ToString("yyyy-MM-dd HH:mm:ss");

                    DateTime createdAtDateTime;
                    DateTime expirationDateTime;
                    if (DateTime.TryParseExact(createdAtString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out createdAtDateTime) &&
                            DateTime.TryParseExact(expirationString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDateTime))
                    {
                        var tokenEntity = new Token
                        {
                            UserId = user.Id,
                            TokenValue = token, 
                            CreatedAt = createdAtDateTime,
                            Expiration = expirationDateTime,
                            IsRevoke = false
                        };
                        await _context.Tokens.AddAsync(tokenEntity);
                        await _context.SaveChangesAsync();
                    }

                    return new APIResponse
                    {
                        Message = "Đăng nhập thành công",
                        Token = token
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Message = "Đăng nhập thất bại",
                        Token = ""
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Message = "Đã xảy ra lỗi trong quá trình đăng nhập",
                    Token = ""
                };
            }
        }

        private string GenerateToken(User user)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);
                if (key == null || key.Length == 0)
                {
                    throw new ArgumentNullException("Jwt:Key", "Khóa JWT không được cấu hình đúng.");
                }
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role),
                };
                _logger.LogInformation("Generating JWT token for user: {User}", user.Username);
                _logger.LogInformation("Claims: {Claims}", string.Join(", ", claims.Select(c => $"{c.Type}: {c.Value}")));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (key), SecurityAlgorithms.HmacSha256Signature),
                    
                };

                _logger.LogInformation("Token Descriptor: Subject={Subject}, Expires={Expires}, SigningCredentials={SigningCredentials}, Issuer={Issuer}, Audience={Audience}",
                tokenDescriptor.Subject, tokenDescriptor.Expires, tokenDescriptor.SigningCredentials, tokenDescriptor.Issuer, tokenDescriptor.Audience);


                var token = jwtTokenHandler.CreateToken(tokenDescriptor);
                return jwtTokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Đã xảy ra lỗi khi tạo mã thông báo JWT", ex);
            }
        }

        public async Task<bool> UnLockAsync(int id)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    user.isLocked = false;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LogoutAsync(string username)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
                if (user != null)
                {

                    var token = await _context.Tokens.SingleOrDefaultAsync(t => t.UserId == user.Id && !t.IsRevoke);
                    if (token != null)
                    {
                        token.IsRevoke = true;
                        _context.Update(token);
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    else return false;
                    
                }
                else return false;
            }
            catch
            {
                return false;
            }

        }
    }
}
