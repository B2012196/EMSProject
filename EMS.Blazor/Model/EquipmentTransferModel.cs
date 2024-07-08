namespace EMS.Blazor.Model
{
    public class EquipmentTransferModel
    {
        public int id { get; set; }
        public DateOnly startDate { get; set; }
        public DateOnly? endDate { get; set; }
        public string note { get; set; }
        public int equipmentId { get; set; }
        public int receivedLocationId { get; set; }
        public int sentLocationId { get; set; }
    }
}
