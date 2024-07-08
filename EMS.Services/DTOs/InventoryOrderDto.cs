using EMS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class InventoryOrderDto
    { 
        public int Id { get; set; } 
        public int ToTalInventory { get; set; }  
    }
}
