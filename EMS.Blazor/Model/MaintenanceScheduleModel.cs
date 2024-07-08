namespace EMS.Blazor.Model
{
    public class MaintenanceScheduleModel
    {
        public int id { get; set; }
        public int equipmentId { get; set; }
        public DateTime scheduleDate { get; set; }
        public string description { get; set; }
        public bool isRepaired { get; set; }
    }
}
