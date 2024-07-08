
using EMS.Services.Implementations;
using EMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EMS.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace EMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _modelService;

        public ModelsController(IModelService modelService)
        {
            _modelService = modelService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _modelService.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return NoContent();
                }
                return Ok(result.Select(r => new
                {
                    Id = r.Id,
                    Name = r.Name
                }));
            }
            catch
            {
                return BadRequest("An error occurred while getting all model.");
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _modelService.GetByIdAsync(id);
                if (data != null)
                    return Ok( new
                    {
                        id = data.Id,
                        Name = data.Name
                    });
                else return NotFound($"No model found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the model by ID.");
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var data = await _modelService.GetByNameAsync(name);
                if (data != null)
                    return Ok(new
                    {
                        id = data.Id,
                        Name = data.Name
                    });
                else return NotFound($"No model found with {name}.");
            }
            catch
            {
                return BadRequest("An error occurred while getting the model by ID.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ModelDto modelDto)
        {
            if(modelDto == null)
            {
                return BadRequest("Model data is null.");
            }
            try
            {
                var result = await _modelService.CreateAsync(modelDto);
                if(result != null)
                {
                    return CreatedAtAction(nameof(GetById), new { id = result.Id }, new
                    {
                        message = "created successfully",
                        id = result.Id
                    });
                }
                else
                    return BadRequest("Unable to create model.");
            }
            catch
            {
                return BadRequest("An error occurred while creating the model.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
                bool result = await _modelService.DeleteAsync(id);
                if (result)
                {
                    return Ok(new
                    {
                        id = id,
                        message = "Deleted successfully."
                    });
                }
                else return NotFound($"No model found with ID {id}.");
            }
            catch
            {
                return BadRequest("An error occurred while deleting the equipment.");
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> Update(ModelDtoUser modelDtoUser)
        {
            try
            {
                bool result = await _modelService.UpdateAsync(modelDtoUser);
                if (result)
                {
                    return Ok(new
                    {
                        id = modelDtoUser.Id,
                        message = "Updated successfully."
                    });
                }
                else return NotFound($"No model found with ID {modelDtoUser.Id}.");
            }
            catch
            {
                return BadRequest("An error occurred while updating the model.");
            }

        }


    }
}
