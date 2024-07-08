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
    public class EquipmentService : IEquipmentService
    {
        private readonly MyDbContext _context;

        public EquipmentService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<Equipment> CreateAsync(EquipmentDto equipmentDto)
        {
            try
            {
                SerialNumberService serialNumberService = new SerialNumberService();
                string checkSeri = serialNumberService.GenerateSerialNumber();
                bool exists = await _context.Equipments.AnyAsync(m => m.Seri == checkSeri);
                while (exists)
                {
                    checkSeri = serialNumberService.GenerateSerialNumber();
                    exists = await _context.Equipments.AnyAsync(m => m.Seri == checkSeri);
                }
                var equipment = new Equipment
                {
                    Name = equipmentDto.Name,
                    Mfg = equipmentDto.Mfg,
                    Seri = checkSeri,
                    Model_Id = equipmentDto.Model_Id,
                    Manufacturer_Id = equipmentDto.Manufacturer_Id,
                    Status_Id = equipmentDto.Status_Id,
                    Location_Id = equipmentDto.Location_Id,
                };
                _context.Add(equipment);
                await _context.SaveChangesAsync();
                
                
                return equipment;
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine($"An error occurred while creating equipment: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            var equipments = await _context.Equipments.ToListAsync();

            return equipments;
        }

        public async Task<Equipment> GetByIdAsync(int id)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == id);
            if (equipment != null)
            {
                return equipment;
            }
            else return null;
        }

        public async Task<List<Equipment>> GetByModelAsync(string modelName)
        {
            var equipments = await _context.Equipments
                                 .Include(e => e.Model)  // Include the related Model entity
                                 .Where(e => e.Model.Name == modelName)
                                 .ToListAsync();
            if (equipments != null)
            {
                return equipments;
            }
            else return null;
        }

        public async Task<List<Equipment>> GetByLocationAsync(string locationname)
        {
            var equipments = await _context.Equipments
                            .Include(e => e.Location)
                            .Where(e => e.Location.Name == locationname)
                            .ToListAsync();
            if (equipments != null)
            {
                return equipments;
            }
            else return null;
        }


        public async Task<Equipment> GetByNameAsync(string name)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Name == name);
            if (equipment != null)
            {
                return equipment;
            }
            else return null;
        }

        public async Task<Equipment> GetBySeriAsync(string seri)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Seri == seri);
            if (equipment != null)
            {
                return equipment;
            }
            else return null;
        }

        public async Task<bool> UpdateAsync(EquipmentDtoUser equipmentDtoUser)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(e => e.Id == equipmentDtoUser.Id);

            if (equipment != null)
            {
                equipment.Name = equipmentDtoUser.Name;
                equipment.Mfg = equipmentDtoUser.Mfg;
                equipment.Manufacturer_Id = equipmentDtoUser.Manufacturer_Id;
                equipment.Model_Id = equipmentDtoUser.Model_Id;
                equipment.Status_Id = equipmentDtoUser.Status_Id;
                equipment.Location_Id = equipmentDtoUser.Location_Id;

                _context.Update(equipment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var equipment = await _context.Equipments.SingleOrDefaultAsync(m => m.Id == id);
            if (equipment != null)
            {
                _context.Remove(equipment);
                await _context.SaveChangesAsync();
                return true;
            }
            else return false;
        }

        public async Task<List<EquipmentStatusDto>> GetAllByStatusIdAsync()
        {
            try
            {
                var equipments = await _context.Equipments
                    .GroupBy(m => m.Status_Id)
                    .Select(g => new EquipmentStatusDto
                    {
                        StatusId = g.Key,
                        EquipmentCount = g.Count()
                    }).ToListAsync();
                if (equipments != null)
                {
                    return equipments;
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
