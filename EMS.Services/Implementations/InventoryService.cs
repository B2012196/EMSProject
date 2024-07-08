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
    public class InventoryService : IInventoryService
    {
        private readonly MyDbContext _context;
        public InventoryService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            try
            {
                var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.Id == id);
                if (inventory != null)
                {
                    return inventory;
                }
                else
                {
                    return null;
                }
            }
            catch { return null; }
        }

        public async Task<List<Inventory>> GetByLocationIdAsync(int locationId)
        {
            try
            {
                var inventories = await _context.Inventories
                    .Where(i => i.LocationId == locationId).ToListAsync();
                if (inventories != null)
                {
                    return inventories;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Inventory> CreateAsync(InventoryDto inventoryDto)
        {
            try
            {
                if (inventoryDto == null) return null;
                else
                {
                    var inventory = new Inventory
                    {
                        Name = inventoryDto.Name,
                        Quatity = inventoryDto.Quatity,
                        LowestQuantity = inventoryDto.LowestQuantity,
                        LocationId = inventoryDto.LocationId,
                        isAccessories = inventoryDto.isAccessories
                    };
                    _context.Add(inventory);
                    await _context.SaveChangesAsync();
                    return inventory;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Inventory>> GetByLocationNameAsync(string locationName)
        {
            try
            {
                var inventory = await _context.Inventories
                    .Include(i => i.Location)
                    .Where(l => l.Location.Name == locationName)
                    .ToListAsync();
                if (inventory != null)
                {
                    return inventory;
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
