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
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("equipment/count/{locationName}")]
        public async Task<ActionResult<int>> GetDeviceCountByDepartment(string locationName)
        {
            try
            {
                var response = await _locationService.GetDeviceCountByDepartment(locationName);
                if(response > 0)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Khong tin thay");
                }
            }
            catch
            {
                return BadRequest("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _locationService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Floor = r.Floor,
                    RoomNumber = r.RoomNumber
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all locations.");
            }
        }
        [HttpGet("distinct")]
        public async Task<IActionResult> GetAllDistinct()
        {
            try
            {
                var result = await _locationService.GetAllDistinctnAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch
            {
                return BadRequest("An error occurred while getting all locations.");
            }
        }

        [HttpGet("equipment-count")]
        public async Task<ActionResult<List<DepartmentEquipmentCountDto>>> GetEquipmentCountByDepartmentAsync()
        {
            try
            {
                var result = await _locationService.GetEquipmentCountByDepartmentAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                else return BadRequest("Error");
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
                var data = await _locationService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Floor = data.Floor,
                        RoomNumber = data.RoomNumber
                    });
                else
                    return NotFound($"No location found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the location by ID.");
            }
        }
        
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var data = await _locationService.GetByNameAsync(name);
                if (data == null || !data.Any())
                {
                    return NotFound($"No location found with name {name}.");
                }
                else
                {
                    return Ok(data.Select(r => new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Floor = r.Floor,
                        RoomNumber = r.RoomNumber
                    }));
                }
            }
            catch
            {
                return BadRequest("An error occurred while getting the location by name.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationDto locationDto)
        {
            if (locationDto == null)
            {
                return BadRequest("Location data is null.");
            }
            try
            {
                var result = await _locationService.CreateAsync(locationDto);
                if (result != null)
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                else
                    return BadRequest("Unable to create location.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the location.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LocationDtoUser locationDtoUser)
        {
            if (locationDtoUser == null)
            {
                return BadRequest("Location data is null.");
            }
            try
            {
                bool result = await _locationService.UpdateAsync(locationDtoUser);
                if (result)
                    return Ok(new
                    {
                        id = locationDtoUser.Id,
                        message = "Updated successfully."
                    });
                else
                    return NotFound($"No location found with id {locationDtoUser.Id}.");
            }
            catch
            {
                return BadRequest("An error occurred while updating the location.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _locationService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        id = id,
                        message = "Deleted successfully."
                    });
                }
                else return NotFound($"No location found with id {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while deleting the location.");
            }

        }
    }
}
