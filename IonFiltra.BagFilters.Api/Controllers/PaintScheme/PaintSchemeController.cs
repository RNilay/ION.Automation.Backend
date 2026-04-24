using IonFiltra.BagFilters.Application.DTOs.PaintScheme;
using IonFiltra.BagFilters.Application.Interfaces.PaintScheme;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.PaintScheme
{
    [Route("api/PaintScheme")]
    [ApiController]
    public class PaintSchemeController : ControllerBase
    {
        private readonly IPaintSchemeService _service;
        private readonly ILogger<PaintSchemeController> _logger;

        public PaintSchemeController(
            IPaintSchemeService service,
            ILogger<PaintSchemeController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ── POST /api/PaintScheme/save ─────────────────────────────────────────
        /// <summary>
        /// Saves a new paint scheme for an enquiry.
        /// Called by the frontend on first-time submit.
        /// </summary>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SavePaintSchemeRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST save: Received a null paint scheme payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "POST save: Saving paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);

                var newId = await _service.SaveAsync(dto);

                return StatusCode(201, new
                {
                    success = true,
                    message = "Paint scheme saved successfully.",
                    data = new { id = newId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error saving paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while saving the paint scheme.",
                    data = (object?)null
                });
            }
        }

        // ── PUT /api/PaintScheme/update ────────────────────────────────────────
        /// <summary>
        /// Replaces an existing paint scheme.
        /// Called by the frontend on subsequent submits (isExistingData = true).
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SavePaintSchemeRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT update: Received a null paint scheme payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT update: Updating paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);

                var updated = await _service.UpdateAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Paint scheme not found for EnquiryId {dto.EnquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Paint scheme updated successfully.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating paint scheme for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating the paint scheme.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/PaintScheme/by-enquiry/{enquiryId} ────────────────────────
        /// <summary>
        /// Returns the full paint scheme for the given enquiry.
        /// Called by the frontend on load to pre-fill the modal.
        /// </summary>
        [HttpGet("by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "GET by-enquiry: Fetching paint scheme for EnquiryId {EnquiryId}", enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = $"No paint scheme found for EnquiryId {enquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Paint scheme fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching paint scheme for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching the paint scheme.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/PaintScheme/exists/{enquiryId} ────────────────────────────
        /// <summary>
        /// Checks whether a paint scheme already exists for the given enquiry.
        /// Optional utility endpoint — the frontend can use this to decide
        /// between save vs update without firing a full load.
        /// </summary>
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
                        ? "Paint scheme exists."
                        : "No paint scheme found.",
                    data = new { exists }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking paint scheme existence for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while checking the paint scheme.",
                    data = (object?)null
                });
            }
        }
    }
}
