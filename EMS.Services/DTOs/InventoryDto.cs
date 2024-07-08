using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class InventoryDto
    {
        public string Name { get; set; }
        public int Quatity { get; set; }
        public int LowestQuantity { get; set; }
        public int LocationId { get; set; }
        public bool isAccessories { get; set; }
    }
}
