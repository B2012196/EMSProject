using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class EquipmentTransferDto
    {
        public int ReceivedLocationId { get; set; }
        public int EquipmentId { get; set; }
        public string Note { get; set; }
    }
}
