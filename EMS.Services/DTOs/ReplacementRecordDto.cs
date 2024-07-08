using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class ReplacementRecordDto
    {
        public int InventoryId { get; set; }
        public int EquipmentId { get; set; }
        public int QuantityUsed { get; set; }
    }
}
