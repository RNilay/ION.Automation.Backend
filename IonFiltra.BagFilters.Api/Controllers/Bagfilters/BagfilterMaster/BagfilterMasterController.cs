using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Api.Controllers.Bagfilters.BagfilterMaster
{
    [ApiController]
    [Route("api/[controller]")]
    public class BagfilterMasterController : ControllerBase
    {
        private readonly IBagfilterMasterService _service;
        private readonly ILogger<BagfilterMasterController> _logger;

        public BagfilterMasterController(
            IBagfilterMasterService service,
            ILogger<BagfilterMasterController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-by-project/{projectId}")]
        public async Task<IActionResult> Get(int projectId)
        {
            _logger.LogInformation("Get started with ProjectId {projectId}", new object[] { projectId });

            try
            {
                var result = await _service.GetByProjectId(projectId);

                if (result == null)
                {
                    _logger.LogWarning("BagfilterMaster with Project ID: {ProjectId} not found.", new object[] { projectId });
                    return Ok(new
                    {
                        success = false,
                        message = $"BagfilterMaster with Project ID {projectId} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for ProjectId {ProjectId}", new object[] { projectId });

                return Ok(new
                {
                    success = true,
                    message = "BagfilterMaster data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching BagfilterMaster with Project ID: {ProjectId}", new object[] { projectId });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BagfilterMasterMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null BagfilterMaster.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new BagfilterMaster.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new BagfilterMaster.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BagfilterMasterMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null BagfilterMaster.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.BagfilterMasterId <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for BagfilterMaster.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating BagfilterMaster with ID: {BagfilterMasterId}", dto.BagfilterMasterId);
                await _service.UpdateAsync(dto);
                return Ok(new { message = "Record updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BagfilterMaster with ID: {BagfilterMasterId}", dto.BagfilterMasterId);
                return StatusCode(500, new { message = "An error occurred while updating the record." });
            }
        }
    }
}
