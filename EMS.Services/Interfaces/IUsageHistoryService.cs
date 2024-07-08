using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IUsageHistoryService
    {
        Task<List<UsageHistory>> GetAllAsync();
        Task<UsageHistory> GetByIdAsync(int id);
        Task<List<EquipmentUsageCountDto>> GetEquipmentUsageStatisticsAsync();
        Task<List<UsageHistory>> GetByUserIdAsync(string name);
        Task<List<UsageHistory>> GetByEquipmentIdAsync(string eqname);
        Task<UsageHistory> CreateAsync(UsageHistoryDto usageHistoryDto);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
