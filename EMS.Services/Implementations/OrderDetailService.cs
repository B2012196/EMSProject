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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly MyDbContext _context;

        public OrderDetailService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDetail> CreateAsync(OrderDetailDto obj)
        {
            try
            {
                if (obj != null)
                {
                    var detail = new OrderDetail()
                    {
                        OrderId = obj.OrderId,
                        InventoryId = obj.InventoryId,
                        Quantity = obj.Quantity,
                    };
                    var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.Id == obj.InventoryId);
                    if (inventory != null)
                    {
                        inventory.Quatity += obj.Quantity;
                        _context.Add(detail);
                        _context.Update(inventory);
                        await _context.SaveChangesAsync();
                        return detail;
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
    }
}
