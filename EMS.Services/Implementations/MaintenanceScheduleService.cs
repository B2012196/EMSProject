using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class MaintenanceScheduleService : IMaintenanceScheduleService
    {
        private readonly MyDbContext _context;

        public MaintenanceScheduleService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<MaintenanceSchedule>> GetAllAsync()
        {
            return await _context.MaintenanceSchedules.ToListAsync();
        }
        public async Task<MaintenanceSummaryDto> GetMaintenanceSummaryAsync()
        {
            var repairedCount = await _context.MaintenanceSchedules.CountAsync(ms => ms.isRepaired);
            var notRepairedCount = await _context.MaintenanceSchedules.CountAsync(ms => !ms.isRepaired);

            return new MaintenanceSummaryDto
            {
                RepairedCount = repairedCount,
                NotRepairedCount = notRepairedCount
            };
        }

        public async Task<List<MaintenanceSchedule>> GetByEquipmentIdAsync(int id)
        {
            try
            {
                var schedule = await _context.MaintenanceSchedules.Where(s => s.EquipmentId == id).ToListAsync();
                if (schedule != null)
                {
                    return schedule;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<MaintenanceSchedule> GetByIdAsync(int id)
        {
            try
            {
                var maintenanceSchedule = await _context.MaintenanceSchedules.SingleOrDefaultAsync(m => m.Id == id);
                if (maintenanceSchedule != null)
                {
                    return maintenanceSchedule;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<MaintenanceSchedule>> GetByEquipmentSeriAsync(string seri)
        {
            try
            {
                var eq = await _context.Equipments.SingleOrDefaultAsync(e => e.Seri == seri);
                if (eq != null)
                {
                    var schedule = await _context.MaintenanceSchedules.Where(s => s.EquipmentId == eq.Id).ToListAsync();
                    if (schedule != null)
                    {
                        return schedule;
                    }
                    else return null;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<MaintenanceSchedule> CreateAsync(MaintenanceScheduleDto MaintenanceScheduleDto)
        {
            try
            {
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == MaintenanceScheduleDto.EquipmentId);
                if (equipment != null && equipment.Status_Id == 1)
                {
                    equipment.Status_Id = 3;

                    //Tao lich bao tri/sua chua
                    var daytime = DateTime.Now;
                    string endtimeString = daytime.ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime DateTimeSchedule;
                    if (DateTime.TryParseExact(endtimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTimeSchedule))
                    { }
                    var schedule = new MaintenanceSchedule
                    {
                        EquipmentId = MaintenanceScheduleDto.EquipmentId,
                        ScheduledDate = DateTimeSchedule,
                        Description = MaintenanceScheduleDto.Description
                    };

                    _context.Add(schedule);
                    _context.Update(equipment);
                    await _context.SaveChangesAsync();

                    return schedule;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var maintenanceSchedule = await _context.MaintenanceSchedules.SingleOrDefaultAsync(m => m.Id == id);
                if (maintenanceSchedule != null)
                {
                    _context.Remove(maintenanceSchedule);
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


        public async Task<bool> UpdateAsync(int id)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.SingleOrDefaultAsync(m => m.Id == id);
            if (maintenanceSchedule != null)
            {
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == maintenanceSchedule.EquipmentId);
                if (equipment != null && equipment.Status_Id == 7)
                {
                    equipment.Status_Id = 3;
                    _context.Equipments.Update(equipment);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var maintenanceSchedule = await _context.MaintenanceSchedules.SingleOrDefaultAsync(m => m.Id == id);
            if (maintenanceSchedule != null)
            {
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == maintenanceSchedule.EquipmentId);
                if (equipment != null && (equipment.Status_Id == 3 || equipment.Status_Id == 7))
                {
                    maintenanceSchedule.isRepaired = true;
                    equipment.Status_Id = 1;
                    equipment.TotalUsageTime = 0;
                    _context.Equipments.Update(equipment);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
