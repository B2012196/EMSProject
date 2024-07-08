using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    UserName = r.Username,
                    Email = r.Email,
                    FullName = r.FullName,
                    JobPosition = r.JobPosition,
                    Role = r.Role,
                    LoginAttempts = r.LoginAttempts,
                    IsLocked = r.isLocked,
                    CreateAt = r.CreatedAt
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all users.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("fullname")]
        public async Task<IActionResult> GetAllFullName()
        {
            try
            {
                var result = await _userService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    UserName = r.Username,
                    Email = r.Email,
                    FullName = r.FullName,
                    JobPosition = r.JobPosition,
                    Role = r.Role,
                    LoginAttempts = r.LoginAttempts,
                    IsLocked = r.isLocked,
                    CreateAt = r.CreatedAt
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all users.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _userService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        UserName = data.Username,
                        Email = data.Email,
                        FullName = data.FullName,
                        JobPosition = data.JobPosition,
                        Role = data.Role,
                        LoginAttempts = data.LoginAttempts,
                        IsLocked = data.isLocked,
                        CreateAt = data.CreatedAt
                    });
                else
                    return NotFound($"No user found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the user by ID.");
            }
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUserName(string username)
        {
            try
            {
                var data = await _userService.GetByUsernameAsync(username);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        UserName = data.Username,
                        Email = data.Email,
                        FullName = data.FullName,
                        JobPosition = data.JobPosition,
                        Role = data.Role,
                        LoginAttempts = data.LoginAttempts,
                        IsLocked = data.isLocked,
                        CreateAt = data.CreatedAt
                    });
                else
                    return NotFound($"No user found with username {username}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the user by username");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var data = await _userService.GetByEmailAsync(email);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        UserName = data.Username,
                        Email = data.Email,
                        FullName = data.FullName,
                        JobPosition = data.JobPosition,
                        Role = data.Role,
                        LoginAttempts = data.LoginAttempts,
                        IsLocked = data.isLocked,
                        CreateAt = data.CreatedAt
                    });
                else
                    return NotFound($"No user found with email {email}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the user by email.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("jobposition/{jobposition}")]
        public async Task<IActionResult> GetByJobPosition(string jobposition)
        {
            try
            {
                var data = await _userService.GetByJobPositionAsync(jobposition);
                if (data == null || !data.Any())
                {
                    return NoContent();
                }
                if (data != null)
                    return Ok(data.Select(r => new
                    {
                        Id = r.Id,
                        UserName = r.Username,
                        Email = r.Email,
                        FullName = r.FullName,
                        JobPosition = r.JobPosition,
                        Role = r.Role,
                        LoginAttempts = r.LoginAttempts,
                        IsLocked = r.isLocked,
                        CreateAt = r.CreatedAt
                    }));
                else
                    return NotFound($"No user found with job position {jobposition}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the user by job position.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }
            try
            {
                var result = await _userService.CreateAsync(userDto);
                if (result != null)
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                else
                    return BadRequest("Unable to create user.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the user.");
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            try
            {
                var result = await _userService.LoginAsync(login);
                if (result.Message == "Đăng nhập thành công" || result.Message == "Đăng nhập thành công với token cũ")
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized(result);


                }
            }
            catch
            {
                return BadRequest("An error occurred while logging in.");
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] string username)
        {
            try
            {
                var result = await _userService.LogoutAsync(username);
                if (result)
                {
                    return Ok("Đăng xuất thành công");
                }
                return BadRequest("Error");
            }
            catch
            {
                return BadRequest("Error");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("unlock")]
        public async Task<IActionResult> UnLock([FromBody] int id)
        {          
            try
            {
                var result = await _userService.UnLockAsync(id);
                if (result != null)
                    return Ok( new
                    {
                        message = "Open lock account successfully",
                        user = id
                    });
                else
                    return BadRequest("Unable to open lock account.");
            }
            catch
            {
                return StatusCode(500, "An internal server error occurred.");
             
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }
            try
            {
                var result = await _userService.UpdateUserAsync(userDto);
                if (result)
                    return CreatedAtAction(nameof(GetById), new { id = userDto.Username }, new
                    {
                        message = "update successfully",
                        id = userDto.Username
                    });
                else
                    return BadRequest("Unable to update user.");
            }
            catch
            {
                return BadRequest("An error occurred while updating the user.");
            }
        }

    }
}
