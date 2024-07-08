using EMS.Data.Entities;
using EMS.Services.DTOs;
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManufacturersController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturersController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _manufacturerService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select( r => new
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all manufacturer.");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _manufacturerService.GetByIdAsync(id);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Address = data.Address,
                        Phone = data.Phone
                    });
                else
                    return NotFound($"No manufacturer found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the manufacturer by ID.");
            }
        }
        
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var data = await _manufacturerService.GetByNameAsync(name);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Address = data.Address,
                        Phone = data.Phone
                    });
                else
                    return NotFound($"No manufacturer found with name {name}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the manufacturer by name.");
            }
        }
        
        [HttpGet("address/{address}")]
        public async Task<IActionResult> GetByAddress(string address)
        {
            try
            {
                var data = await _manufacturerService.GetByAddressAsync(address);
                if (data == null || !data.Any())
                    return NotFound($"No manufacturer found with address {address}.");
                else {
                    return Ok(data.Select(r => new
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Address = r.Address,
                        Phone = r.Phone
                    }));
                }               
            }
            catch
            {
                return BadRequest("An error occurred while getting the manufacturer by address.");
            }
        }
        
        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            try
            {
                var data = await _manufacturerService.GetByPhoneAsync(phone);
                if (data != null)
                    return Ok(new
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Address = data.Address,
                        Phone = data.Phone
                    });
                else
                    return NotFound($"No manufacturer found with phone {phone}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the manufacturer by phone.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ManufacturerDto manufacturerDto)
        {
            if (manufacturerDto == null)
            {
                return BadRequest("Manufacturer data is null.");
            }
            try
            {
                var result = await _manufacturerService.CreateAsync(manufacturerDto);
                if (result != null) {
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                }
                else
                    return BadRequest("Unable to create manufacturer.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the manufacturer.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ManufacturerDtoUser manufacturerDtoUser)
        {
            try
            {
                bool result = await _manufacturerService.UpdateAsync(manufacturerDtoUser);
                if (result)
                    return Ok(new
                    {
                        id = manufacturerDtoUser.Id,
                        message = "Updated successfully."
                    });
                else
                    return NotFound($"No equipment found with id {manufacturerDtoUser.Id}.");
            }
            catch
            {
                return BadRequest("An error occurred while updating the manufacturer.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                bool result = await _manufacturerService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        id = id,
                        message = "Deleted successfully."
                    });
                }
                else return NotFound($"No manufacturer found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while deleting the manufacturer.");
            }

        }
    }
}
