using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Services.DTOs
{
    public class MaintenanceSummaryDto
    {
        public int RepairedCount { get; set; }
        public int NotRepairedCount { get; set; }
    }
}
