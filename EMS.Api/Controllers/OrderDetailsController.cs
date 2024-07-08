using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;


        public OrderDetailsController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] List<OrderDetailDto> detailDto)
        {
            try
            {
                if (detailDto != null)
                {
                    foreach (var detail in detailDto)
                    {
                        var result = await _orderDetailService.CreateAsync(detail);
                        if(result == null)
                            return BadRequest("Error");
                    }
                    return Ok("Created successfully");
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch
            {
                return BadRequest("Error");
            }


        }
    }
}
