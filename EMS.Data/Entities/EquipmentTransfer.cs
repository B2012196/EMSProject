using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class EquipmentTransfer
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Note { get; set; }
        public int EquipmentId { get; set; }
        public int ReceivedLocationId { get; set; } // Khóa ngoại tới ReceivedLocation
        public int SentLocationId { get; set; } // Khóa ngoại tới SentLocation

        public Location ReceivedLocation { get; set; }
        public Location SentLocation { get; set; }
        public Equipment Equipment { get; set; }
    }

}
