using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceSchedulesController : ControllerBase
    {
        private readonly IMaintenanceScheduleService _maintenanceScheduleService;

        public MaintenanceSchedulesController(IMaintenanceScheduleService maintenanceScheduleService)
        {
            _maintenanceScheduleService = maintenanceScheduleService;
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _maintenanceScheduleService.GetAllAsync();
                if (!result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    EquipmentId = r.EquipmentId,
                    ScheduleDate = r.ScheduledDate,
                    Description = r.Description,
                    isRepaired = r.isRepaired
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all equipment.");
            }
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetMaintenanceSummary()
        {
            try
            {
                var summary = await _maintenanceScheduleService.GetMaintenanceSummaryAsync();
                if (summary != null)
                {
                    return Ok(summary);
                }
                else return BadRequest("ERROR");
            }
            catch
            {
                return BadRequest("ERROR");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _maintenanceScheduleService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        EquipmentId = data.EquipmentId,
                        ScheduleDate = data.ScheduledDate,
                        Description = data.Description,
                        isRepaired = data.isRepaired
                    });
                else
                    return NotFound($"No equipment found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by ID.");
            }
        }

        [HttpGet("seri/{seri}")]
        public async Task<IActionResult> GetByEquipmentSeri(string seri)
        {
            try
            {
                var result = await _maintenanceScheduleService.GetByEquipmentSeriAsync(seri);
                if (result != null)
                {
                    return Ok(result.Select(r => new
                    {
                        Id = r.Id,
                        EquipmentId = r.EquipmentId,
                        ScheduleDate = r.ScheduledDate,
                        Description = r.Description,
                        isRepaired = r.isRepaired
                    }));
                }
                else return BadRequest("Not found");
            }
            catch
            {
                return BadRequest("An error occurred while getting all equipment.");
            }
        }


        [HttpGet("equipmentid/{id}")]
        public async Task<IActionResult> GetByEquipmentId(int id)
        {
            try
            {
                var result = await _maintenanceScheduleService.GetByEquipmentIdAsync(id);
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    EquipmentId = r.EquipmentId,
                    ScheduleDate = r.ScheduledDate,
                    Description = r.Description,
                    isRepaired = r.isRepaired
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by ID.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaintenanceScheduleDto maintenanceScheduleDto)
        {
            if (maintenanceScheduleDto == null)
            {
                return BadRequest("Maintenance schedule data is null");
            }
            try
            {
                var result = await _maintenanceScheduleService.CreateAsync(maintenanceScheduleDto);
                if (result != null)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                }
                else return BadRequest("Unable to create maintenance schedule");
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                Console.Error.WriteLine($"Database update exception: {dbEx.Message}");
                return StatusCode(500, "A database error occurred while creating the equipment.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred while creating equipment: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the equipment.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                bool result = await _maintenanceScheduleService.UpdateAsync(id);
                if (result)
                    return Ok(new
                    {
                        id = id,
                        message = "Update the equipment is being repaired successfully"
                    });
                else
                    return NotFound("Unable to update equipment status");
            }
            catch
            {
                return BadRequest("An error occurred while updating the equipment.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("complete")]
        public async Task<IActionResult> Complete([FromBody] int id)
        {
            try
            {
                bool result = await _maintenanceScheduleService.CompleteAsync(id);
                if (result)
                    return Ok(new
                    {
                        id = id,
                        message = "Update the equipment has been repaired successfully"
                    });
                else
                    return NotFound("Unable to update equipment status");
            }
            catch
            {
                return BadRequest("An error occurred while updating the equipment.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            try
            {
                bool result = await _maintenanceScheduleService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        id = id,
                        message = "Deleted successfully."
                    });
                }
                else return NotFound($"No Maintenamce Schedule found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while deleting the equipment.");
            }
        }

    }
}
