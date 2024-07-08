namespace EMS.Blazor.Model
{
    public class InventoryModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int quatity { get; set; }
        public int lowestQuantity { get; set; }
        public int locationId { get; set; }
        public bool isAccessories { get; set; }
    }
}
