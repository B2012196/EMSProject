using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IEquipmentTransferService
    {
        Task<List<EquipmentTransfer>> GetAllAsync();
        Task<EquipmentTransfer> GetByIdAsync(int id);
        Task<EquipmentTransfer> GetBySeriEquipmentAsync(string seri);
        Task<EquipmentTransfer> CreateAsync(EquipmentTransferDto equipmentTransferDto);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
