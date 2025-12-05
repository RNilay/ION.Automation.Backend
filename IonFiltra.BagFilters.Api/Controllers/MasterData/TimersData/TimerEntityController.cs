using IonFiltra.BagFilters.Application.DTOs.MasterData.TimerData;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.MasterData.TimerData
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimerEntityController : ControllerBase
    {
        private readonly ITimerEntityService _service;
        private readonly ILogger<TimerEntityController> _logger;

        public TimerEntityController(
            ITimerEntityService service,
            ILogger<TimerEntityController> logger)
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
                    _logger.LogWarning("TimerEntity with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"TimerEntity with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "TimerEntity data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching TimerEntity with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] TimerEntityMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null TimerEntity.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new TimerEntity.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new TimerEntity.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TimerEntityMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null TimerEntity.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for TimerEntity.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating TimerEntity with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating TimerEntity with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll TimerEntity started");

            try
            {
                var result = await _service.GetAll();

                if (result == null || !result.Any())
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No TimerEntity found.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "TimerEntity list fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching TimerEntity.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }

    }
}
    