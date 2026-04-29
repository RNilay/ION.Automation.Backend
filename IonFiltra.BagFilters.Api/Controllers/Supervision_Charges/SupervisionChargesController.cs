using IonFiltra.BagFilters.Application.DTOs.Supervision_Charges;
using IonFiltra.BagFilters.Application.Interfaces.Supervision_Charges;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.Supervision_Charges
{
    [Route("api/SupervisionCharges")]
    [ApiController]
    public class SupervisionChargesController : ControllerBase
    {
        private readonly ISupervisionChargesService _service;
        private readonly ILogger<SupervisionChargesController> _logger;

        public SupervisionChargesController(
            ISupervisionChargesService service,
            ILogger<SupervisionChargesController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ── POST /api/SupervisionCharges/save ─────────────────────────
        [HttpPost("save")]
        public async Task<IActionResult> Save(
            [FromBody] SaveSupervisionChargesRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST save: Received null supervision charges payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "POST save: Saving supervision charges for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                var newId = await _service.SaveAsync(dto);

                return StatusCode(201, new
                {
                    success = true,
                    message = "Supervision charges saved successfully.",
                    data = new { id = newId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error saving supervision charges for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while saving supervision charges.",
                    data = (object?)null
                });
            }
        }

        // ── PUT /api/SupervisionCharges/update ────────────────────────
        [HttpPut("update")]
        public async Task<IActionResult> Update(
            [FromBody] SaveSupervisionChargesRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT update: Received null supervision charges payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT update: Updating supervision charges for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                var updated = await _service.UpdateAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Supervision charges not found for EnquiryId {dto.EnquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Supervision charges updated successfully.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating supervision charges for EnquiryId {EnquiryId}",
                    dto.EnquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating supervision charges.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/SupervisionCharges/by-enquiry/{enquiryId} ────────
        [HttpGet("by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "GET by-enquiry: Fetching supervision charges for EnquiryId {EnquiryId}",
                enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = $"No supervision charges found for EnquiryId {enquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Supervision charges fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching supervision charges for EnquiryId {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching supervision charges.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/SupervisionCharges/exists/{enquiryId} ────────────
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
                        ? "Supervision charges exist."
                        : "No supervision charges found.",
                    data = new { exists }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking supervision charges existence for EnquiryId {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while checking supervision charges.",
                    data = (object?)null
                });
            }
        }
    }
}
