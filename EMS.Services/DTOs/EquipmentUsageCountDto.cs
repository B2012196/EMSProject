using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class EquipmentUsageCountDto
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentSeri { get; set; }
        public int UsageCount { get; set; }
    }
}
