namespace EMS.Blazor.Model
{
    public class InventoryDto
    {
        public string Name { get; set; }
        public int Quatity { get; set; }
        public int LowestQuantity { get; set; }
        public int LocationId { get; set; }
        public bool isAccessories { get; set; }
    }
}
