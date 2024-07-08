namespace EMS.Blazor.Model
{
    public class ReplacementRecordModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int EquipmentId { get; set; }
        public DateOnly ReplacementDate { get; set; }
        public int QuantityUsed { get; set; }
    }
}
