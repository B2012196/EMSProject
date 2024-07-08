using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class UsageHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double UsageDuration => EndTime.HasValue ? (EndTime.Value - StartTime).TotalMinutes : 0;
        public User User { get; set; }
        public Equipment Equipment { get; set; }
    }
}
