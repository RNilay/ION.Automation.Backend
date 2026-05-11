using IonFiltra.BagFilters.Application.DTOs.ApprovedMakes;
using IonFiltra.BagFilters.Application.Interfaces.ApprovedMakes;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.ApprovedMakes
{
    [Route("api/ApprovedMakes")]
    [ApiController]
    public class ApprovedMakesController : ControllerBase
    {
        private readonly IApprovedMakesService _service;
        private readonly ILogger<ApprovedMakesController> _logger;

        public ApprovedMakesController(
            IApprovedMakesService service,
            ILogger<ApprovedMakesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ── POST /api/ApprovedMakes/save ───────────────────────────────────────
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveApprovedMakesRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST save: Received a null approved makes payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "POST save: Saving approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);

                var newId = await _service.SaveAsync(dto);

                return StatusCode(201, new
                {
                    success = true,
                    message = "Approved makes saved successfully.",
                    data = new { id = newId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error saving approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while saving approved makes.",
                    data = (object?)null
                });
            }
        }

        // ── PUT /api/ApprovedMakes/update ──────────────────────────────────────
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SaveApprovedMakesRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT update: Received a null approved makes payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT update: Updating approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);

                var updated = await _service.UpdateAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Approved makes not found for EnquiryId {dto.EnquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Approved makes updated successfully.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating approved makes for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating approved makes.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/ApprovedMakes/by-enquiry/{enquiryId} ──────────────────────
        [HttpGet("by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "GET by-enquiry: Fetching approved makes for EnquiryId {EnquiryId}", enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = $"No approved makes found for EnquiryId {enquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Approved makes fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching approved makes for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching approved makes.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/ApprovedMakes/exists/{enquiryId} ──────────────────────────
        [HttpGet("exists/{enquiryId}")]
        public async Task<IActionResult> Exists(int enquiryId)
        {
            try
            {
                var exists = await _service.ExistsByEnquiryIdAsync(enquiryId);

                return Ok(new
                {
                    success = true,
                    message = exists ? "Approved makes exist." : "No approved makes found.",
                    data = new { exists }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking approved makes existence for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while checking approved makes.",
                    data = (object?)null
                });
            }
        }
    }
}
