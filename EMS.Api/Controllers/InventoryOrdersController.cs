using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryOrdersController : ControllerBase
    {
        private readonly IInventoryOrderService _inventoryOrderService;

        public InventoryOrdersController(IInventoryOrderService inventoryOrderService)
        {
            _inventoryOrderService = inventoryOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _inventoryOrderService.GetAllAsync();
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                else
                {
                    return Ok(response.Select(r => new
                    {
                        Id = r.Id,
                        OrderDate = r.OrderDate,
                        ToTalInventory = r.ToTalInventory
                    }));
                }
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _inventoryOrderService.GetByIdAsync(id);
                if(response != null)
                {
                    return Ok(new
                    {
                        Id = response.Id,
                        OrderDate = response.OrderDate,
                        ToTalInventory = response.ToTalInventory
                    });
                }
                else return NotFound($"No order found with ID {id}.");
            }
            catch
            {
                return BadRequest("Error");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] int ToTalInventory)
        {
            try
            {
                var response = await _inventoryOrderService.CreateAsync(ToTalInventory);

                if (response != null)
                {
                    return Ok(new
                    {
                        Id = response.Id,
                        Message = "Created successfully"

                    });
                }
                else return BadRequest("Error");
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] InventoryOrderDto obj)
        {
            try
            {
                var response = await _inventoryOrderService.UpdateAsync(obj);
                if(response == true)
                {
                    return Ok("Updated successfully");
                }
                else return BadRequest("Error");
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _inventoryOrderService.DeleteAsync(id);
                if (response == true)
                {
                    return Ok("Deleted sucessfully");
                }
                else return BadRequest("Error");
            }
            catch
            {
                return BadRequest("Error");
            }
        }
    }
}
