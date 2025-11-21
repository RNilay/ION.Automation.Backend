using IonFiltra.BagFilters.Application.DTOs.BOM.Rates;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.BOM.Rates
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillOfMaterialRatesController : ControllerBase
    {
        private readonly IBillOfMaterialRatesService _service;
        private readonly ILogger<BillOfMaterialRatesController> _logger;

        public BillOfMaterialRatesController(
            IBillOfMaterialRatesService service,
            ILogger<BillOfMaterialRatesController> logger)
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
                    _logger.LogWarning("BillOfMaterialRates with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"BillOfMaterialRates with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "BillOfMaterialRates data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching BillOfMaterialRates with Project ID: {Id}", new object[] { id });
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
            _logger.LogInformation("GetAll started for BillOfMaterialRates");

            try
            {
                var result = await _service.GetAll();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No BillOfMaterialRates found.");
                    return Ok(new
                    {
                        success = false,
                        message = "No BillOfMaterialRates found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("GetAll completed successfully.");

                return Ok(new
                {
                    success = true,
                    message = "BillOfMaterialRates list fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching list of BillOfMaterialRates");
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BillOfMaterialRatesMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null BillOfMaterialRates.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new BillOfMaterialRates.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new BillOfMaterialRates.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BillOfMaterialRatesMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null BillOfMaterialRates.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for BillOfMaterialRates.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating BillOfMaterialRates with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BillOfMaterialRates with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
    }
}
    