using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface ILocationService
    {
        Task<List<Location>> GetAllAsync();
        Task<List<string>> GetAllDistinctnAsync();
        Task<List<DepartmentEquipmentCountDto>> GetEquipmentCountByDepartmentAsync();
        Task<Location> GetByIdAsync(int id);
        Task<List<Location>> GetByNameAsync(string name);
        Task<Location> CreateAsync(LocationDto locationDto);
        Task<bool> UpdateAsync(LocationDtoUser locationDtoUser);
        Task<bool> DeleteAsync(int id);
        Task<int> GetDeviceCountByDepartment(string name);
    }
}
