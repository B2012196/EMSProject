using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsageHistoriesController : ControllerBase
    {
        private readonly IUsageHistoryService _usageHistoryService;

        public UsageHistoriesController(IUsageHistoryService usageHistoryService)
        {
            _usageHistoryService = usageHistoryService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _usageHistoryService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    EquipmentId = r.EquipmentId,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    UsageDuration = r.UsageDuration
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all Usage Histories.");
            }
        }

        [HttpGet("equipment-usage-statistics")]
        public async Task<ActionResult<List<EquipmentUsageCountDto>>> GetEquipmentUsageStatistics()
        {
            var result = await _usageHistoryService.GetEquipmentUsageStatisticsAsync();
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _usageHistoryService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        UserId = data.UserId,
                        EquipmentId = data.EquipmentId,
                        StartTime = data.StartTime,
                        Endtime = data.EndTime,
             
                        UsageDuration = data.UsageDuration
                    });
                else
                    return NotFound($"No Usage Histories found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the Usage Histories by ID.");
            }
        }

 
        [HttpGet("userid/{name}")]
        public async Task<IActionResult> GetByUserId(string name)
        {
            try
            {
             
                var result = await _usageHistoryService.GetByUserIdAsync(name);
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                if (result != null)
                    return Ok(result.Select(r => new
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        EquipmentId = r.EquipmentId,
                        StartTime = r.StartTime,
                        EndTime = r.EndTime,
                        UsageDuration = r.UsageDuration
                    }));
                else
                    return NotFound($"No Usage Histories found with Equipment name {name}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the Usage Histories by full name.");
            }
        }

    
        [HttpGet("equipmentid/{eqname}")]
        public async Task<IActionResult> GetByEquipmentId(string eqname)
        {
            try
            {
                var data = await _usageHistoryService.GetByEquipmentIdAsync(eqname);
                if (data != null)
                    return Ok(data.Select(r => new
                    {
                        Id = r.Id,
                        UserId = r.UserId,
                        EquipmentId = r.EquipmentId,
                        StartTime = r.StartTime,
                        EndTime = r.EndTime,
         
                        UsageDuration = r.UsageDuration
                    }));
                else
                    return NotFound($"No Usage Histories found with name {eqname}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the Usage Histories by Equipment name.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsageHistoryDto usageHistoryDto)
        {
            var user = User;
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Contains("User") && !roles.Contains("Admin"))
            {
                return Forbid();
            } 

            try
            {
                var result = await _usageHistoryService.CreateAsync(usageHistoryDto);
                if (result != null)
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                else
                    return BadRequest("Cannot use the equipment");
            }
            catch
            {
                return BadRequest("An error occurred while creating the Usage History.");
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("complete")]
        public async Task<IActionResult> Complete([FromBody] int id)
        {
            try
            {
                var result = await _usageHistoryService.CompleteAsync(id);
                if (result)
                    return Ok(new
                    {
                        message = "Updated successfully",
                        id = id
                    });
                else
                    return BadRequest("Unable to complete Usage History.");
            }
            catch
            {
                return BadRequest("An error occurred while completing the Usage History.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _usageHistoryService.DeleteAsync(id);
                if (result)
                    return Ok(new
                    {
                        message = "Deleted successfully",
                        id = id
                    });
                else
                    return BadRequest("Unable to complete Usage History.");
            }
            catch
            {
                return BadRequest("An error occurred while completing the Usage History.");
            }
        }

    }
}
