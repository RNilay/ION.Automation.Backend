using IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.BOM.Cage_Cost
{
    [ApiController]
    [Route("api/[controller]")]
    public class CageCostEntityController : ControllerBase
    {
        private readonly ICageCostEntityService _service;
        private readonly ILogger<CageCostEntityController> _logger;

        public CageCostEntityController(
            ICageCostEntityService service,
            ILogger<CageCostEntityController> logger)
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
                    _logger.LogWarning("CageCostEntity with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"CageCostEntity with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "CageCostEntity data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching CageCostEntity with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("get-by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation("Get started for EnquiryId {EnquiryId}", enquiryId);

            try
            {
                var result = await _service.GetByEnquiryId(enquiryId);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning(
                        "No CageCostEntity records found for EnquiryId: {EnquiryId}",
                        enquiryId);

                    return Ok(new
                    {
                        success = true, // empty list is NOT an error
                        message = $"No CageCostEntity records found for EnquiryId {enquiryId}.",
                        data = Array.Empty<CageCostMainDto>()
                    });
                }

                _logger.LogInformation(
                    "Get completed successfully for EnquiryId {EnquiryId}. Count: {Count}",
                    enquiryId,
                    result.Count);

                return Ok(new
                {
                    success = true,
                    message = "CageCostEntity data fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while fetching CageCostEntity for EnquiryId: {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CageCostMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null CageCostEntity.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new CageCostEntity.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new CageCostEntity.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] CageCostMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null CageCostEntity.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for CageCostEntity.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating CageCostEntity with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating CageCostEntity with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
    }
}
    