using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<List<Manufacturer>> GetAllAsync();
        Task<Manufacturer> GetByIdAsync(int id);
        Task<Manufacturer> GetByNameAsync(string name);
        Task<List<Manufacturer>> GetByAddressAsync(string address);
        Task<Manufacturer> GetByPhoneAsync(string phone);
        Task<Manufacturer> CreateAsync(ManufacturerDto manufacturerDto);
        Task<bool> UpdateAsync(ManufacturerDtoUser manufacturerDtoUser);
        Task<bool> DeleteAsync(int id);
    }
}
