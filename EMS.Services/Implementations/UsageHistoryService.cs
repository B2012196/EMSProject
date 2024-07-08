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
    public class UsageHistoryService : IUsageHistoryService
    {
        private readonly MyDbContext _context;

        public UsageHistoryService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<UsageHistory>> GetAllAsync()
        {
            return await _context.UsageHistories.ToListAsync();
        }

        public async Task<List<UsageHistory>> GetByEquipmentIdAsync(string eqname)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Name == eqname);
            if (equipment != null)
            {
                var usagehistory = await _context.UsageHistories.Where(u => u.EquipmentId == equipment.Id).ToListAsync();
                if (usagehistory != null)
                {
                    return usagehistory;
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<EquipmentUsageCountDto>> GetEquipmentUsageStatisticsAsync()
        {
            var result = await _context.UsageHistories
                .GroupBy(uh => new { uh.EquipmentId, uh.Equipment.Name, uh.Equipment.Seri })
                .Select(group => new EquipmentUsageCountDto
                {
                    EquipmentId = group.Key.EquipmentId,
                    EquipmentName = group.Key.Name,
                    EquipmentSeri = group.Key.Seri,
                    UsageCount = group.Count()
                }).ToListAsync();
            return result;
        }

        public async Task<UsageHistory> GetByIdAsync(int id)
        {
            var usagehistory = await _context.UsageHistories.SingleOrDefaultAsync(u => u.Id == id);
            if (usagehistory != null)
            {
                return usagehistory;
            }
            else return null;
        }

        public async Task<List<UsageHistory>> GetByUserIdAsync(string name)
        {
            var username = await _context.Users.SingleOrDefaultAsync(u => u.FullName == name);
            if (username != null)
            {
                var usagehistory = await _context.UsageHistories.Where(u => u.UserId == username.Id).ToListAsync();
                if (usagehistory != null)
                {
                    return usagehistory;
                }
                else return null;
            }
            else return null;
        }
        public async Task<bool> CompleteAsync(int id)
        {
            var usagehistory = await _context.UsageHistories.SingleOrDefaultAsync(u => u.Id == id);
            if (usagehistory != null && usagehistory.EndTime == null)
            {
                var endtimekt = DateTime.Now;
                string endtimeString = endtimekt.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime endDateTime;
                if (DateTime.TryParseExact(endtimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateTime))
                {
                    usagehistory.EndTime = endDateTime;
                }

                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == usagehistory.EquipmentId);
                if (equipment != null)
                {
                    equipment.TotalUsageTime += usagehistory.UsageDuration;
                    equipment.Status_Id = 1;
                    //sử dụng quá 50 minutes
                    if (equipment.TotalUsageTime > 50)
                    {
                        equipment.Status_Id = 7;
                        equipment.TotalUsageTime = 0;
                        var maintenanceSchedule = new MaintenanceSchedule
                        {
                            EquipmentId = equipment.Id,
                            ScheduledDate = endDateTime,
                            Description = "Scheduled maintenance after exceeding 50 minutes of usage."
                        };

                        _context.MaintenanceSchedules.Add(maintenanceSchedule);
                    }
                    _context.Equipments.Update(equipment);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<UsageHistory> CreateAsync(UsageHistoryDto usageHistoryDto)
        {
            try
            {
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == usageHistoryDto.EquipmentId);
                if (equipment == null || equipment.Status_Id != 1)
                {
                    return null;
                }
                var starttimebd = DateTime.Now;
                string starttimeString = starttimebd.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime startDateTime;
                if (DateTime.TryParseExact(starttimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateTime))
                {

                    var usagehistory = new UsageHistory
                    {
                        UserId = usageHistoryDto.UserId,
                        EquipmentId = usageHistoryDto.EquipmentId,
                        StartTime = startDateTime,
                        EndTime = null,
                    };
                    equipment.Status_Id = 2;
                    _context.Equipments.Update(equipment);
                    await _context.UsageHistories.AddAsync(usagehistory);
                    await _context.SaveChangesAsync();
                    return usagehistory;
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
                var usagehistory = await _context.UsageHistories.SingleOrDefaultAsync(u => u.Id == id);
                if (usagehistory != null)
                {
                    _context.Remove(usagehistory);
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
    }
}
