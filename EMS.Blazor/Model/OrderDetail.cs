namespace EMS.Blazor.Model
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
    }
}
