using IonFiltra.BagFilters.Application.DTOs.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.MasterData.SolenoidValveData
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolenoidValveController : ControllerBase
    {
        private readonly ISolenoidValveService _service;
        private readonly ILogger<SolenoidValveController> _logger;

        public SolenoidValveController(
            ISolenoidValveService service,
            ILogger<SolenoidValveController> logger)
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
                    _logger.LogWarning("SolenoidValve with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"SolenoidValve with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "SolenoidValve data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching SolenoidValve with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] SolenoidValveMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null SolenoidValve.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new SolenoidValve.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new SolenoidValve.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SolenoidValveMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null SolenoidValve.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for SolenoidValve.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating SolenoidValve with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating SolenoidValve with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();

            return Ok(new
            {
                success = true,
                message = "SolenoidValve list fetched successfully.",
                data = result
            });
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
                    message = "SolenoidValve deleted successfully (soft delete).",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting SolenoidValve {Id}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while deleting the record."
                });
            }
        }

    }
}
    