using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZXing;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoriesController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _inventoryService.GetAllAsync();
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                else
                {
                    return Ok(response.Select(r => new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Quatity = r.Quatity,
                        LowestQuantity = r.LowestQuantity,
                        LocationId = r.LocationId,
                        isAccessories = r.isAccessories
                    }));
                }
            }
            catch
            {
                return BadRequest("An error occurred while getting all inventories.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _inventoryService.GetByIdAsync(id);
                if(response != null)
                {
                    return Ok(new
                    {
                        Id = response.Id,
                        Name = response.Name,
                        Quatity = response.Quatity,
                        LowestQuantity = response.LowestQuantity,
                        LocationId = response.LocationId,
                        isAccessories = response.isAccessories
                    });
                }
                else return NotFound($"No inventory found with ID {id}.");
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet("location/{locationid}")]
        public async Task<IActionResult> GetByLocationId(int locationid)
        {
            try
            {
                var response = await _inventoryService.GetByLocationIdAsync(locationid);
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                else
                {
                    return Ok(response.Select(r => new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Quatity = r.Quatity,
                        LowestQuantity = r.LowestQuantity,
                        LocationId = r.LocationId,
                        isAccessories = r.isAccessories
                    }));
                }
            }
            catch
            {
                return BadRequest("An error occurred while getting inventories by location Id.");
            }
        }
        
        [HttpGet("locationName/{locationName}")]
        public async Task<IActionResult> GetByLocationName(string locationName)
        {
            try
            {
                var response = await _inventoryService.GetByLocationNameAsync(locationName);
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                else
                {
                    return Ok(response.Select(r => new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Quatity = r.Quatity,
                        LowestQuantity = r.LowestQuantity,
                        LocationId = r.LocationId,
                        isAccessories = r.isAccessories
                    }));
                }
            }
            catch
            {
                return BadRequest("An error occurred while getting inventories by location Id.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InventoryDto obj)
        {
            try
            {
                if (obj == null)
                {
                    return BadRequest("Equipment data is null");
                }
                var result = await _inventoryService.CreateAsync(obj);
                if (result != null) {
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                }
                else return BadRequest("Unable create inventory");
            }
            catch
            {
                return BadRequest("Error");
            }
        }
    }
}
