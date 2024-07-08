using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quatity { get; set; }
        public int LowestQuantity { get; set; }
        public int LocationId { get; set; }
        public bool isAccessories { get; set; }
        public Location Location { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<ReplacementRecord> ReplacementRecords { get; set; }
    }
}
