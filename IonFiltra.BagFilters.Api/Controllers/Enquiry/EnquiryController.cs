using IonFiltra.BagFilters.Application.DTOs.Enquiry;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace IonFiltra.BagFilters.Api.Controllers.Enquiry
{
    /// <summary>
    /// API Controller for managing Design Parameters.
    /// </summary>
    [Route("api/enquiries")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _service;
        private readonly ILogger<EnquiryController> _logger;

        public EnquiryController(IEnquiryService service, ILogger<EnquiryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] EnquiryMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null Enquiry.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new Enquiry.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new Enquiry.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpGet("get-by-userId/{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            _logger.LogInformation("GET: Fetching Enquiry with User ID: {UserId}", new object[] { userId });

            try
            {
                var result = await _service.GetByUserId(userId);

                if (result == null)
                {
                    _logger.LogWarning("Enquiry not found for User ID: {UserId}", new object[] { userId });
                    return Ok(new
                    {
                        success = false,
                        message = $"Enquiry with User ID {userId} was not found.",
                        data = (object?)null
                    });
                }

                _logger.LogInformation("GET completed successfully for User ID: {UserId}", new object[] { userId });

                return Ok(new
                {
                    success = true,
                    message = "Enquiry data fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred while fetching Enquiry for UserId {userId}: {ex.Message}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }

        [HttpGet("pagenatedEnquiries/{userId}")]
        public async Task<IActionResult> GetByUserIdPaged(int userId, [FromQuery] int pageNumber, [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation("GET: Fetching paginated Enquiries for User ID: {UserId}, Page: {PageNumber}", userId, pageNumber);

            try
            {
                var (items, totalCount) = await _service.GetByUserId(userId, pageNumber, pageSize);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning("No Enquiries found for User ID: {UserId}, Page: {PageNumber}", userId, pageNumber);
                    return Ok(new
                    {
                        success = false,
                        message = $"No enquiries found for User ID {userId}.",
                        data = new { items = new List<EnquiryMainDto>(), totalCount = 0 }
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Enquiries fetched successfully.",
                    data = new { items, totalCount }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching Enquiries for User ID: {UserId}, Page: {PageNumber}", userId, pageNumber);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }



        /// <summary>
        /// Returns paginated enquiries for ALL users EXCEPT the specified userId.
        /// Used by the "All Enquiries" tab on the dashboard so the current user
        /// can view and act on other users' enquiries without seeing their own.
        /// </summary>
        [HttpGet("pagenatedEnquiriesExcludingUser/{userId}")]
        public async Task<IActionResult> GetAllExcludingUser(
            int userId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation(
                "GET: Fetching paginated Enquiries excluding User ID: {UserId}, Page: {PageNumber}",
                userId, pageNumber);

            try
            {
                var (items, totalCount) = await _service.GetAllExceptUserId(userId, pageNumber, pageSize);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning(
                        "No Enquiries found (excluding User ID: {UserId}), Page: {PageNumber}",
                        userId, pageNumber);

                    return Ok(new
                    {
                        success = false,
                        message = "No enquiries found for other users.",
                        data = new { items = new List<EnquiryMainDto>(), totalCount = 0 }
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Enquiries fetched successfully.",
                    data = new { items, totalCount }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred while fetching Enquiries excluding User ID: {UserId}, Page: {PageNumber}",
                    userId, pageNumber);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }




        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] EnquiryMainDto dto)
        {
            if (dto == null || dto.Enquiry == null)
            {
                _logger.LogWarning("PUT: Received null Enquiry update payload.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation(
                    "PUT: Updating Enquiry {EnquiryId} for User {UserId}",
                    dto.Enquiry.EnquiryId,
                    dto.UserId
                );

                var updated = await _service.UpdateByEnquiryIdAsync(dto);

                if (!updated)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Enquiry not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Enquiry updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating Enquiry.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        // ── DELETE /api/enquiries/soft-delete/{enquiryId} ─────────────────────
        /// <summary>
        /// Soft-deletes an enquiry by its business-key EnquiryId.
        /// Sets IsDeleted = true and records DeletedAt timestamp.
        /// The row is never physically removed from the database.
        /// </summary>
        [HttpDelete("soft-delete/{enquiryId}")]
        public async Task<IActionResult> SoftDelete(string enquiryId)
        {
            if (string.IsNullOrWhiteSpace(enquiryId))
            {
                _logger.LogWarning("DELETE: Received empty enquiryId.");
                return BadRequest("EnquiryId cannot be empty.");
            }

            try
            {
                _logger.LogInformation(
                    "DELETE: Soft-deleting Enquiry {EnquiryId}", enquiryId);

                var deleted = await _service.SoftDeleteByEnquiryIdAsync(enquiryId);

                if (!deleted)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Enquiry '{enquiryId}' not found or already deleted."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Enquiry deleted successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error occurred while soft-deleting Enquiry {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request."
                });
            }
        }

    }
}
