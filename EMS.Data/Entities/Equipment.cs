using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Seri { get; set; }
        public DateOnly Mfg { get; set; }
        public DateOnly Purchase_Date { get; set; }
        public int Model_Id { get; set; }
        public int Manufacturer_Id { get; set; }
        public int Status_Id { get; set; }
        public int Location_Id { get; set; }
        public double TotalUsageTime { get; set; }

        //Foreign key relationship
        public Model Model { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Status Status { get; set; }
        public Location Location { get; set; }
        public ICollection<EquipmentTransfer> EquipmentTransfers { get; set; }
        public ICollection<UsageHistory> UsageHistories { get; set; }
        public ICollection<MaintenanceSchedule> MaintenanceSchedules { get; set; }
        public ICollection<ReplacementRecord> ReplacementRecords { get; set; }
    }
}
