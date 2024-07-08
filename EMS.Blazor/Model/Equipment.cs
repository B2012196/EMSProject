namespace EMS.Blazor.Model
{
    public class Equipment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string seri { get; set; }
        public DateOnly mfg { get; set; }
        public DateOnly purchase_Date { get; set; }
        public int model_Id { get; set; }
        public int manufacturer_Id { get; set; }
        public int status_Id { get; set; }
        public int location_Id { get; set; }
    }
}
