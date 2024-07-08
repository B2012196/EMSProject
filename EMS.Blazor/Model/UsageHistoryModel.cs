namespace EMS.Blazor.Model
{
    public class UsageHistoryModel
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int equipmentId { get; set; }
        public DateTime startTime { get; set; }
        public DateTime? endTime { get; set; }
        
        public double usageDuration => endTime.HasValue ? (endTime.Value - startTime).TotalMinutes : 0;
    }
}
