namespace EMS.Blazor.Model
{
    public class UserModel
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string fullName { get; set; }
        public string jobPosition { get; set; }
        public string role { get; set; }
        public int loginAttempts { get; set; }
        public bool isLocked { get; set; }
        public DateOnly createAt { get; set; }
    }
}
