using EMS.Data;
using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly MyDbContext _context;

        public StatusService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Status>> GetAllAsync()
        {
            return await _context.Status.ToListAsync();
        }

        public async Task<Status> GetByIdAsync(int id)
        {
            try
            {
                var status = await _context.Status.SingleOrDefaultAsync(s => s.Id == id);
                if (status != null)
                {
                    return status;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Status> CreateAsync(StatusDto statusDto)
        {
            if (string.IsNullOrEmpty(statusDto.Name))
            {
                return null;
            }

            var status = new Status
            {
                Name = statusDto.Name,
            };

            _context.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var status = await _context.Status.SingleOrDefaultAsync(s => s.Id == id);
                if (status != null)
                {
                    _context.Status.Remove(status);
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
