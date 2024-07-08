using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStatistics()
        {
            // Đây là dữ liệu giả lập. Thay đổi nó thành dữ liệu thực tế của bạn.
            var statistics = new List<double> { 10.5, 20.3, 30.2, 40.7, 50.1 };
            return Ok(statistics);
        }
    }
}
