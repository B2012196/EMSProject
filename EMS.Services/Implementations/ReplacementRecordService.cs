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
    public class ReplacementRecordService : IReplacementRecordService
    {
        private readonly MyDbContext _context;

        public ReplacementRecordService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<ReplacementRecord> CreateAsync(ReplacementRecordDto replacementRecordDto)
        {
            try
            {
                var record = new ReplacementRecord()
                {
                    InventoryId = replacementRecordDto.InventoryId,
                    EquipmentId = replacementRecordDto.EquipmentId,
                    QuantityUsed = replacementRecordDto.QuantityUsed,
                };
                var inventory = await _context.Inventories
                    .SingleOrDefaultAsync(i => i.Id == replacementRecordDto.InventoryId && i.Quatity > 0);
                if (inventory != null)
                {
                    inventory.Quatity -= replacementRecordDto.QuantityUsed;
                    _context.Add(record);
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                    return record;
                }
                else return null;
                
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ReplacementRecord>> GetAllAsync()
        {
            return await _context.ReplacementRecords.ToListAsync();
        }

        public async Task<ReplacementRecord> GetByIdAsync(int id)
        {
            try
            {
                var record = await _context.ReplacementRecords.SingleOrDefaultAsync(r => r.Id == id);
                if (record != null)
                {
                    return record;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
