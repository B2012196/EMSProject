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
    public class EquipmentTransferService : IEquipmentTransferService
    {
        private readonly MyDbContext _context;

        public EquipmentTransferService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<EquipmentTransfer>> GetAllAsync()
        {
            return await _context.EquipmentTransfers.ToListAsync();

        }

        public async Task<EquipmentTransfer> GetByIdAsync(int id)
        {
            var transfer = await _context.EquipmentTransfers.SingleOrDefaultAsync(e => e.Id == id);

            if (transfer != null)
            {
                return transfer;
            }
            else return null;
        }
        public async Task<EquipmentTransfer> GetBySeriEquipmentAsync(string seri)
        {
            try
            {
                var eq = await _context.Equipments.SingleOrDefaultAsync(eq => eq.Seri == seri);
                if (eq != null)
                {
                    var transfer = await _context.EquipmentTransfers.SingleOrDefaultAsync(t => t.EquipmentId == eq.Id);
                    if (transfer != null)
                    {
                        return transfer;
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

        public async Task<bool> CompleteAsync(int id)
        {
            var equipmentTransfer = await _context.EquipmentTransfers.SingleOrDefaultAsync(e => e.Id == id);
            if (equipmentTransfer != null && equipmentTransfer.EndDate == null)
            {
                equipmentTransfer.EndDate = DateOnly.FromDateTime(DateTime.Today);
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == equipmentTransfer.EquipmentId);
                if (equipment != null) {
                    equipment.Status_Id = 1;
                    equipment.Location_Id = equipmentTransfer.ReceivedLocationId;
                    _context.Update(equipment);
                    _context.Update(equipmentTransfer);
                    await _context.SaveChangesAsync();
                    return true;
                }                   
                else return false;  
            }
            else return false;
        }

        public async Task<EquipmentTransfer> CreateAsync(EquipmentTransferDto equipmentTransferDto)
        {
            try
            {
                var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == equipmentTransferDto.EquipmentId);
                
                if (equipment != null)
                {
                    if (equipment.Status_Id != 1 )
                    {
                        return null;
                    }
                    else
                    {
                        var equipmentTransfer = new EquipmentTransfer
                        {
                            SentLocationId = equipment.Location_Id,
                            ReceivedLocationId = equipmentTransferDto.ReceivedLocationId,
                            EquipmentId = equipmentTransferDto.EquipmentId,
                            EndDate = null,
                            Note = equipmentTransferDto.Note

                        };
                        equipment.Status_Id = 6;
                        _context.Add(equipmentTransfer);
                        _context.Update(equipment);
                        await _context.SaveChangesAsync();
                        return equipmentTransfer;
                    }
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
