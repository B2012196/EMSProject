using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class EquipmentDto
    {
        public string Name { get; set; }
        public DateOnly Mfg { get; set; }
        public int Model_Id { get; set; }
        public int Manufacturer_Id { get; set; }
        public int Status_Id { get; set; }
        public int Location_Id { get; set; }
    }
}
