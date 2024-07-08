using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IInventoryOrderService
    {
        Task<List<InventoryOrder>> GetAllAsync();
        Task<InventoryOrder> GetByIdAsync(int id);
        Task<InventoryOrder> CreateAsync(int ToTalInventory);
        Task<bool> UpdateAsync(InventoryOrderDto obj);
        Task<bool> DeleteAsync(int id);
    }
}
