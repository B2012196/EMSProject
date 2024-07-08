namespace EMS.Blazor.Model
{
    public class InventoryOrderModel
    {
        public int id { get; set; }
        public DateOnly orderDate { get; set; }
        public int toTalInventory { get; set; }
    }
}
