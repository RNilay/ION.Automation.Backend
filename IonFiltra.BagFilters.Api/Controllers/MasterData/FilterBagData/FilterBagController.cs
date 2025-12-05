using IonFiltra.BagFilters.Application.DTOs.MasterData.FilterBagData;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.MasterData.FilterBagData
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilterBagController : ControllerBase
    {
        private readonly IFilterBagService _service;
        private readonly ILogger<FilterBagController> _logger;

        public FilterBagController(
            IFilterBagService service,
            ILogger<FilterBagController> logger)
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
                    _logger.LogWarning("FilterBag with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"FilterBag with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "FilterBag data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching FilterBag with Project ID: {Id}", new object[] { id });
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
            _logger.LogInformation("GetAll FilterBag started");

            try
            {
                var result = await _service.GetAll();

                if (result == null || !result.Any())
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No FilterBag found.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "FilterBag list fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching FilterBag.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] FilterBagMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null FilterBag.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new FilterBag.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new FilterBag.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] FilterBagMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null FilterBag.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for FilterBag.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating FilterBag with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating FilterBag with ID: {Id}", dto.Id);
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
                _logger.LogError(ex, "Error deleting FilterBag.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while deleting the record."
                });
            }
        }


    }
}
    