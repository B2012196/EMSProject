using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class MaintenanceSchedule
    {
        public int Id { get; set; }
        public int EquipmentId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string Description { get; set; }
        public bool isRepaired { get; set; }
        public Equipment Equipment { get; set; }


    }
}
