using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class Location 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public string RoomNumber { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<EquipmentTransfer> SentTransfers { get; set; }
        public ICollection<EquipmentTransfer> ReceivedTransfers { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
