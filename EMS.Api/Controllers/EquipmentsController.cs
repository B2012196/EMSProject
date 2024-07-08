using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZXing;

namespace EMS.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentsController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _equipmentService.GetAllAsync();
                if(result == null || !result.Any())
                {
                    return NoContent();
                }
                var response = result.Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    Seri = e.Seri,
                    Mfg = e.Mfg,
                    Purchase_Date = e.Purchase_Date,
                    Model_Id = e.Model_Id,
                    Manufacturer_Id = e.Manufacturer_Id,
                    Status_Id = e.Status_Id,
                    Location_Id = e.Location_Id,
                });
                return Ok(response);
            }
            catch (Exception ex) 
            {
                Console.Error.WriteLine($"An error occurred while getting all equipment: {ex.Message}");
                return BadRequest("An error occurred while getting all equipment.");
            }
        }

        [HttpGet("equipmentCount")]
        public async Task<IActionResult> GetEquipmentCountByStatusId()
        {
            try
            {
                var response = await _equipmentService.GetAllByStatusIdAsync();
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                else if (response != null)
                {
                    return Ok(response);
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
                var data = await _equipmentService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Seri = data.Seri,
                        Mfg = data.Mfg,
                        Purchase_Date = data.Purchase_Date,
                        Model_Id = data.Model_Id,
                        Manufacturer_Id = data.Manufacturer_Id,
                        Status_Id = data.Status_Id,
                        Location_Id = data.Location_Id,
                    });
                else 
                    return NotFound($"No equipment found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by ID.");
            }
        }
        
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var data = await _equipmentService.GetByNameAsync(name);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Seri = data.Seri,
                        Mfg = data.Mfg,
                        Purchase_Date = data.Purchase_Date,
                        Model_Id = data.Model_Id,
                        Manufacturer_Id = data.Manufacturer_Id,
                        Status_Id = data.Status_Id,
                        Location_Id = data.Location_Id,
                    });
                else 
                    return NotFound($"No equipment found with name {name}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by name.");
            }
        }
        
        [HttpGet("seri/{seri}")]
        public async Task<IActionResult> GetBySeri(string seri)
        {
            try
            {
                var data = await _equipmentService.GetBySeriAsync(seri);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Seri = data.Seri,
                        Mfg = data.Mfg,
                        Purchase_Date = data.Purchase_Date,
                        Model_Id = data.Model_Id,
                        Manufacturer_Id = data.Manufacturer_Id,
                        Status_Id = data.Status_Id,
                        Location_Id = data.Location_Id,
                    });
                else
                    return NotFound($"No equipment found with serial number {seri}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by serial number.");
            }
        }


        [HttpGet("model/{model}")]
        public async Task<IActionResult> GetByModel(string model)
        {
            try
            {
                var data = await _equipmentService.GetByModelAsync(model);
                if (data != null && data.Any()) { 
                    var response = data.Select(e => new
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Seri = e.Seri,
                        Mfg = e.Mfg,
                        Purchase_Date = e.Purchase_Date,
                        Model_Id = e.Model_Id,
                        Manufacturer_Id = e.Manufacturer_Id,
                        Status_Id = e.Status_Id,
                        Location_Id = e.Location_Id,
                    });
                return Ok(response);
                    }
                else
                    return NotFound($"No equipment found with model {model}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the equipment by model.");
            }
        }

        [HttpGet("locationname/{name}")]
        public async Task<IActionResult> GetByLocation(string name)
        {
            try
            {
                var data = await _equipmentService.GetByLocationAsync(name);
                if (data != null && data.Any())
                {
                    var response = data.Select(e => new
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Seri = e.Seri,
                        Mfg = e.Mfg,
                        Purchase_Date = e.Purchase_Date,
                        Model_Id = e.Model_Id,
                        Manufacturer_Id = e.Manufacturer_Id,
                        Status_Id = e.Status_Id,
                        Location_Id = e.Location_Id,
                    });
                    return Ok(response);
                }
                else
                    return NotFound($"No location found with location {name}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the location by location.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentDto equipmentDto)
        {
            if(equipmentDto == null)
            {
                return BadRequest("Equipment data is null");
            }
            try
            {
                var result = await _equipmentService.CreateAsync(equipmentDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { 
                    message = "created successfully",
                    id = result.Id });
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                bool result = await _equipmentService.DeleteAsync(id);
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
        
        
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EquipmentDtoUser equipmentDtoUser)
        {
            try
            {
                bool result = await _equipmentService.UpdateAsync(equipmentDtoUser);
                if (result)
                    return Ok(new
                    {
                        id = equipmentDtoUser.Id,
                        message = "Updated successfully."
                    });
                else
                    return NotFound($"No equipment found with id {equipmentDtoUser.Id}.");
            }
            catch
            {
                return BadRequest("An error occurred while updating the equipment.");
            }
        }

    }
}
