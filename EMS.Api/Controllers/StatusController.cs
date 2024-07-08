using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _statusService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all status.");
            }
        }

    
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _statusService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                    });
                else return NotFound($"No status found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the status by ID.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(StatusDto statusDto)
        {
            if (statusDto == null)
            {
                return BadRequest("Status data is null");
            }
            try
            {
                var result = await _statusService.CreateAsync(statusDto);
                if (result != null)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                }
                else
                    return BadRequest("Unable to create status.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the status.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                bool result = await _statusService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        id = id,
                        message = "Deleted successfully."
                    });
                }
                else return NotFound($"No equipment found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while deleting the equipment.");
            }

        }
    }
}
