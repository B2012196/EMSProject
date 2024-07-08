using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class ReplacementRecord
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public Inventory Inventory { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public DateOnly ReplacementDate { get; set; }
        public int QuantityUsed { get; set; }
    }
}
