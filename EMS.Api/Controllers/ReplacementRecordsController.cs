using EMS.Services.DTOs;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ZXing;

namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplacementRecordsController : ControllerBase
    {
        private readonly IReplacementRecordService _replacementRecordService;

        public ReplacementRecordsController(IReplacementRecordService replacementRecordService)
        {
            _replacementRecordService = replacementRecordService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var response = await _replacementRecordService.GetAllAsync();
                if (response == null || !response.Any())
                {
                    return NoContent();
                }
                return Ok(response.Select(e => new
                {
                    Id = e.Id,
                    InventoryId = e.InventoryId,
                    EquipmentId = e.EquipmentId,
                    QuantityUsed = e.QuantityUsed,
                    ReplacementDate = e.ReplacementDate,
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all ReplacementRecord");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var response = await _replacementRecordService.GetByIdAsync(id);
                if(response != null)
                {
                    return Ok(new
                    {
                        Id = response.Id,
                        InventoryId = response.InventoryId,
                        EquipmentId = response.EquipmentId,
                        QuantityUsed = response.QuantityUsed,
                        ReplacementDate = response.ReplacementDate,
                    });
                }
                else return NotFound($"No replacement record found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the replacement record by ID.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReplacementRecordDto replacementRecordDto)
        {
            try
            {
                if (replacementRecordDto == null)
                {
                    return BadRequest("ReplacementRecordService data is null");
                }
                var response = await _replacementRecordService.CreateAsync(replacementRecordDto);
                if (response != null)
                {
                    return Ok(new
                    {
                        message = "created successfully",
                        id = response.Id
                    });
                }
                else return BadRequest("Số lượng phụ tùng không đủ");
            }
            catch (Exception ex) 
            {
                return BadRequest("Error" + ex.Message);
            }
        }



    }
}
