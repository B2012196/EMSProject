using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory> GetByIdAsync(int id);
        Task<List<Inventory>> GetByLocationIdAsync(int locationId);
        Task<List<Inventory>> GetByLocationNameAsync(string locationName);
        Task<Inventory> CreateAsync(InventoryDto inventoryDto);
    }
}
