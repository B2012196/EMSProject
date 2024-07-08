using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetByJobPositionAsync(string jobPosition);
        Task<User> CreateAsync(UserDto UserDto);
        Task<bool> UpdateUserAsync(UserDto UserDto);
        Task<bool> DeleteAsync(string username);
        Task<bool> UnLockAsync(int id);
        Task<APIResponse> LoginAsync(Login login);
        Task<bool> LogoutAsync(string username);
    }
}
