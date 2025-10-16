using IonFiltra.BagFilters.Application.DTOs.Assignment;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.Assignment
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {

        private readonly IAssignmentEntityService _service;
        private readonly ILogger<AssignmentController> _logger;

        public AssignmentController(
            IAssignmentEntityService service,
            ILogger<AssignmentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AssignmentRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("POST: Received a null Assignment request.");
                return BadRequest(new
                {
                    success = false,
                    message = "Request body cannot be null.",
                    data = (object?)null
                });
            }

            try
            {
                _logger.LogInformation("POST: Adding Assignments for EnquiryId {EnquiryId}", request.EnquiryId);

                var createdAssignments = await _service.AddAsync(request);

                return StatusCode(201, new
                {
                    success = true,
                    message = $"{createdAssignments.Count} assignments created successfully.",
                    data = createdAssignments
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding new Assignments.");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }

        [HttpGet("get-by-userId/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            _logger.LogInformation("GET: Fetching Assignments for User ID: {UserId}", userId);

            try
            {
                var result = await _service.GetByUserId(userId);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("Assignments not found for User ID: {UserId}", userId);
                    return Ok(new
                    {
                        success = false,
                        message = $"Assignments for User ID {userId} were not found.",
                        data = (object?)null
                    });
                }

                _logger.LogInformation("GET completed successfully for User ID: {UserId}", userId);

                return Ok(new
                {
                    success = true,
                    message = "Assignments fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Assignments for User ID: {UserId}", userId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }


        [HttpGet("pagenatedAssignments/{userId}")]
        public async Task<IActionResult> GetByUserIdPaged(
            int userId,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation("GET: Fetching paginated Assignments for User ID: {UserId}, Page: {PageNumber}", userId, pageNumber);

            try
            {
                var (items, totalCount) = await _service.GetByUserId(userId, pageNumber, pageSize);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning("Assignments not found for User ID: {UserId}, Page {PageNumber}", userId, pageNumber);
                    return Ok(new
                    {
                        success = false,
                        message = $"Assignments for User ID {userId} were not found.",
                        data = new { items = new List<AssignmentMainDto>(), totalCount = 0 }
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Assignments fetched successfully.",
                    data = new { items, totalCount }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Assignments for User ID: {UserId}, Page {PageNumber}", userId, pageNumber);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }


        [HttpGet("pagenatedAssignmentsByEnquiry/{enquiryId}")]
        public async Task<IActionResult> GetByEnquiryIdPaged(
        string enquiryId,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize = 8)
        {
            _logger.LogInformation("GET: Fetching paginated Assignments for User ID: {UserId}, Page: {PageNumber}", enquiryId, pageNumber);

            try
            {
                var (items, totalCount) = await _service.GetByEnquiryId(enquiryId, pageNumber, pageSize);

                if (items == null || !items.Any())
                {
                    _logger.LogWarning("Assignments not found for User ID: {UserId}, Page {PageNumber}", enquiryId, pageNumber);
                    return Ok(new
                    {
                        success = false,
                        message = $"Assignments for User ID {enquiryId} were not found.",
                        data = new { items = new List<AssignmentMainDto>(), totalCount = 0 }
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Assignments fetched successfully.",
                    data = new { items, totalCount }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Assignments for User ID: {EnquiryId}, Page {PageNumber}", enquiryId, pageNumber);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }



    }



}
