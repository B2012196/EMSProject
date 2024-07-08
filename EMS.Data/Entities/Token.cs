using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Data.Entities
{
    public class Token
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TokenValue { get; set; } //chuoi token
        public DateTime CreatedAt { get; set; } // Thoi gian  tao
        public DateTime Expiration { get; set; } // Thoi gian het han
        public bool IsRevoke { get; set; } // trang thai

        public User User { get; set; }
    }
}
