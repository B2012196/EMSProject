using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string JobPosition { get; set; } 
        public string Role { get; set; } 
        public int LoginAttempts { get; set; } 
        public bool isLocked { get; set; }
        public DateOnly CreatedAt { get; set; }
        public ICollection<Token> Tokens { get; set; }
        public ICollection<UsageHistory> UsageHistories { get; set; }
    }
}
