using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class LocationDtoUser
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public string RoomNumber { get; set; }
    }
}
