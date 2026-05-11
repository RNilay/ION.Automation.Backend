using IonFiltra.BagFilters.Application.DTOs.DuctEngineering;
using IonFiltra.BagFilters.Application.Interfaces.DuctEngineering;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.DuctEngineering
{
    [Route("api/DuctEngineering")]
    [ApiController]
    public class DuctEngineeringController : ControllerBase
    {
        private readonly IDuctEngineeringService _service;
        private readonly ILogger<DuctEngineeringController> _logger;

        public DuctEngineeringController(
            IDuctEngineeringService service,
            ILogger<DuctEngineeringController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ── POST /api/DuctEngineering/save ─────────────────────────────────
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveDuctEngineeringRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST save: Received a null duct engineering payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "POST save: Saving duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);

                var newId = await _service.SaveAsync(dto);

                return StatusCode(201, new
                {
                    success = true,
                    message = "Duct engineering settings saved successfully.",
                    data = new { id = newId }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error saving duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while saving duct engineering settings.",
                    data = (object?)null
                });
            }
        }

        // ── PUT /api/DuctEngineering/update ────────────────────────────────
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] SaveDuctEngineeringRequestDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT update: Received a null duct engineering payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT update: Updating duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);

                var updated = await _service.UpdateAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Duct engineering record not found for EnquiryId {dto.EnquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Duct engineering settings updated successfully.",
                    data = (object?)null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating duct engineering for EnquiryId {EnquiryId}", dto.EnquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while updating duct engineering settings.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/DuctEngineering/by-enquiry/{enquiryId} ───────────────
        [HttpGet("by-enquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "GET by-enquiry: Fetching duct engineering for EnquiryId {EnquiryId}", enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                if (result == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = $"No duct engineering record found for EnquiryId {enquiryId}.",
                        data = (object?)null
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Duct engineering settings fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error fetching duct engineering for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching duct engineering settings.",
                    data = (object?)null
                });
            }
        }

        // ── GET /api/DuctEngineering/exists/{enquiryId} ───────────────────
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
                        ? "Duct engineering record exists."
                        : "No duct engineering record found.",
                    data = new { exists }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error checking duct engineering existence for EnquiryId {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while checking duct engineering.",
                    data = (object?)null
                });
            }
        }
    }
}
