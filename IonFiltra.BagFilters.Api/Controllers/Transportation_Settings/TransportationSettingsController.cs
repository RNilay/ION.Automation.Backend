using IonFiltra.BagFilters.Application.DTOs.Transportation_Settings;
using IonFiltra.BagFilters.Application.Interfaces.Transportation_Settings;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.Transportation_Settings
{
    [Route("api/TransportationGlobalSettings")]
    [ApiController]
    public class TransportationSettingsController : ControllerBase
    {
        private readonly ITransportationSettingsService _service;
        private readonly ILogger<TransportationSettingsController> _logger;

        public TransportationSettingsController(
            ITransportationSettingsService service,
            ILogger<TransportationSettingsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ── POST /api/TransportationGlobalSettings/save ───────────────
        [HttpPost("save")]
        public async Task<IActionResult> Save(
            [FromBody] SaveTransportationSettingsRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST save: Received null transportation settings payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "POST save: Saving transportation settings for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                var newId = await _service.SaveAsync(dto);

                return StatusCode(201, new
                {
                    success = true,
                    message = "Transportation settings saved successfully.",
                    data = new { id = newId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error saving transportation settings for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while saving transportation settings.",
                    data = (object?)null
                });
            }
        }

        // ── PUT /api/TransportationGlobalSettings/update ──────────────
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromBody] SaveTransportationSettingsRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT update: Received null transportation settings payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT update: Updating transportation settings for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                var updated = await _service.UpdateAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Transportation settings not found for EnquiryId {dto.EnquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Transportation settings updated successfully.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating transportation settings for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating transportation settings.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/TransportationGlobalSettings/by-enquiry/{id} ─────
        [HttpGet("by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "GET by-enquiry: Fetching transportation settings for EnquiryId {EnquiryId}",
                enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = $"No transportation settings found for EnquiryId {enquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Transportation settings fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching transportation settings for EnquiryId {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching transportation settings.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/TransportationGlobalSettings/exists/{id} ─────────
        [HttpGet("exists/{enquiryId}")]
        public async Task<IActionResult> Exists(int enquiryId)
        {
            try
            {
                var exists = await _service.ExistsByEnquiryIdAsync(enquiryId);

                return Ok(new
                {
                    success = true,
                    message = exists
                        ? "Transportation settings exist."
                        : "No transportation settings found.",
                    data = new { exists }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking transportation settings existence for EnquiryId {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while checking transportation settings.",
                    data = (object?)null
                });
            }
        }
    }
}
