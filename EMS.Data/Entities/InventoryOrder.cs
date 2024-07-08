using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class InventoryOrder
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public int ToTalInventory { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
