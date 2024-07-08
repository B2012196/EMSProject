using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Services.Interfaces;

namespace EMS.Services.Implementations
{
    public class SerialNumberService
    {
        public string GenerateSerialNumber()
        {
            var random = new Random();
            string serialNumber = string.Empty;

            for (int i = 0; i < 10; i++)
            {
                serialNumber += random.Next(0, 10).ToString();
            }

            return serialNumber;
        }
    }
}
