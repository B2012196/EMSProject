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
    public class InventoryOrderService : IInventoryOrderService
    {
        private readonly MyDbContext _context;

        public InventoryOrderService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<InventoryOrder> CreateAsync(int ToTalInventory)
        {
            try
            {
                var inorder = new InventoryOrder()
                {
                    ToTalInventory = ToTalInventory,
                };
                _context.Add(inorder);
                await _context.SaveChangesAsync();
                return inorder;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateAsync(InventoryOrderDto obj)
        {
            try
            {
                var inorder = await _context.InventoryOrders.SingleOrDefaultAsync(i => i.Id == obj.Id);
                if (inorder != null)
                {
                    inorder.ToTalInventory = obj.ToTalInventory;
                    _context.Update(inorder);
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

        public async Task<List<InventoryOrder>> GetAllAsync()
        {
            return await _context.InventoryOrders.ToListAsync();
        }

        public async Task<InventoryOrder> GetByIdAsync(int id)
        {
            try
            {
                var order = await _context.InventoryOrders.SingleOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    return order;
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
                var order = await _context.InventoryOrders.SingleOrDefaultAsync(o => o.Id == id);
                if (order != null)
                {
                    _context.Remove(order);
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
