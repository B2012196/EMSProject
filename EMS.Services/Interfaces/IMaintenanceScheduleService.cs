using EMS.Data.Entities;
using EMS.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Interfaces
{
    public interface IMaintenanceScheduleService
    {
        Task<List<MaintenanceSchedule>> GetAllAsync();
        Task<MaintenanceSchedule> GetByIdAsync(int id);
        Task<List<MaintenanceSchedule>> GetByEquipmentSeriAsync(string seri);
        Task<List<MaintenanceSchedule>> GetByEquipmentIdAsync(int id);
        Task<MaintenanceSchedule> CreateAsync(MaintenanceScheduleDto MaintenanceScheduleDto);
        Task<bool> UpdateAsync(int id);
        Task<bool> CompleteAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<MaintenanceSummaryDto> GetMaintenanceSummaryAsync();
    }
}
