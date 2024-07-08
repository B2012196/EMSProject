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
    public class EquipmentTransfersController : ControllerBase
    {
        private readonly IEquipmentTransferService _equipmentTransferService;

        public EquipmentTransfersController(IEquipmentTransferService equipmentTransferService)
        {
            _equipmentTransferService = equipmentTransferService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _equipmentTransferService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(e => new
                {
                    Id = e.Id,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    ReceivedLocationId = e.ReceivedLocationId,
                    SentLocationId = e.SentLocationId,
                    EquipmentId = e.EquipmentId,
                    Note = e.Note
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all equipment transfer.");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _equipmentTransferService.GetByIdAsync(id);
                if (data != null)
                    return Ok( new
                    {
                        Id = data.Id,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        ReceivedLocationId = data.ReceivedLocationId,
                        SentLocationId = data.SentLocationId,
                        EquipmentId = data.EquipmentId,
                        Note = data.Note
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
        public async Task<IActionResult> GetBySeriEquipment(string seri)
        {
            try
            {
                var data = await _equipmentTransferService.GetBySeriEquipmentAsync(seri);
                if(data != null)
                {
                    return Ok(new
                    {
                        Id = data.Id,
                        StartDate = data.StartDate,
                        EndDate = data.EndDate,
                        ReceivedLocationId = data.ReceivedLocationId,
                        SentLocationId = data.SentLocationId,
                        EquipmentId = data.EquipmentId,
                        Note = data.Note
                    });
                }
                else
                    return NotFound($"No equipment found.");
            }
            catch
            {
                return BadRequest("An error occurred while getting equipment transfer.");
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentTransferDto equipmentTransferDto)
        {
            try
            {
                if (equipmentTransferDto == null)
                {
                    return BadRequest("Equipment data is null");
                }
                var result = await _equipmentTransferService.CreateAsync(equipmentTransferDto);
                if (result != null)
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                else
                    return BadRequest("Unable to create equipment transfer.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the equipment transfer.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Complete([FromBody]int id)
        {
            try
            {
                var result = await _equipmentTransferService.CompleteAsync(id);
                if (result)
                    return Ok( new 
                    {
                        message = "Updated successfully",
                        id = id
                    });
                else
                    return BadRequest("Unable to update equipment transfer");
            }
            catch
            {
                return BadRequest("An error occurred while updating the equipment transfer.");
            }
        }



    }
}
