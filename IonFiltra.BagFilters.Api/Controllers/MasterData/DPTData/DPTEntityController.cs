using IonFiltra.BagFilters.Application.DTOs.MasterData.DPTData;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.MasterData.DPTData
{
    [ApiController]
    [Route("api/[controller]")]
    public class DPTEntityController : ControllerBase
    {
        private readonly IDPTEntityService _service;
        private readonly ILogger<DPTEntityController> _logger;

        public DPTEntityController(
            IDPTEntityService service,
            ILogger<DPTEntityController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Get started with Id {id}", new object[] { id });

            try
            {
                var result = await _service.GetById(id);

                if (result == null)
                {
                    _logger.LogWarning("DPTEntity with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"DPTEntity with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "DPTEntity data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching DPTEntity with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }
        
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll DPTEntity started");

            try
            {
                var result = await _service.GetAll();

                if (result == null || !result.Any())
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No DPTEntity found.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "DPTEntity list fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching DPTEntity.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] DPTMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null DPTEntity.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new DPTEntity.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new DPTEntity.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] DPTMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null DPTEntity.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for DPTEntity.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating DPTEntity with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating DPTEntity with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
        
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID.");

            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    success = true,
                    message = "Record deleted successfully (soft delete)."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting DPTEntity.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while deleting the record."
                });
            }
        }
    }
}
    