namespace EMS.Blazor.Model
{
    public class Locations
    {
        public int id { get; set; }
        public string name { get; set; }
        public int floor { get; set; }
        public string roomNumber { get; set; }

        public string DisplayName => $"{name} - Tầng: {floor} - Phòng: {roomNumber}";
    }
}
