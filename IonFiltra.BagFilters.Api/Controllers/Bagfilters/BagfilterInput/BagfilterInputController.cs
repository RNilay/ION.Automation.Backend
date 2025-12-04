using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.API.Controllers.Bagfilters.BagfilterInputs
{
    [ApiController]
    [Route("api/[controller]")]
    public class BagfilterInputController : ControllerBase
    {
        private readonly IBagfilterInputService _service;
        private readonly ILogger<BagfilterInputController> _logger;

        public BagfilterInputController(
            IBagfilterInputService service,
            ILogger<BagfilterInputController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-by-master-id/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Get started with Id {id}", new object[] { id });

            try
            {
                var result = await _service.GetByMasterId(id);

                if (result == null)
                {
                    _logger.LogWarning("BagfilterInput with Project ID: {ProjectId} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"BagfilterInput with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "BagfilterInput data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching BagfilterInput with ID: {id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("get-all/{enquiryId}")]
        public async Task<IActionResult> GetAll(int enquiryId)
        {
            try
            {
                var result = await _service.GetAllByEnquiryId(enquiryId);

                return Ok(new
                {
                    success = true,
                    message = "Bagfilter data fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Bagfilters for enquiry {id}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "Internal server error.",
                    data = (object?)null
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BagfilterInputMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null BagfilterInput.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new BagfilterInput.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new BagfilterInput.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        //old code
        //[HttpPost("add-batch")]
        //public async Task<IActionResult> AddBatch([FromBody] List<BagfilterInputMainDto> dtos)
        //{
        //    if (dtos == null || dtos.Count == 0)
        //    {
        //        _logger.LogWarning("POST: Received empty BagfilterInput batch.");
        //        return BadRequest("Request body cannot be empty.");
        //    }

        //    try
        //    {
        //        _logger.LogInformation("POST: Adding batch of BagfilterInputs. Count={Count}", dtos.Count);
        //        var createdIds = await _service.AddRangeAsync(dtos); // returns list of created BagfilterInputIds (or master ids)
        //        return StatusCode(201, new { createdInputIds = createdIds });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding BagfilterInput batch.");
        //        return StatusCode(500, "An error occurred while processing your request.");
        //    }
        //}

        [HttpPost("add-batch")]
        public async Task<IActionResult> AddBatch([FromBody] List<BagfilterInputMainDto> dtos, CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0)
                return BadRequest("Request body cannot be empty.");

            try
            {
                var result = await _service.AddRangeAsync(dtos, ct);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding BagfilterInput batch.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut("update-batch")]
        public async Task<IActionResult> UpdateBatch([FromBody] List<BagfilterInputMainDto> dtos, CancellationToken ct)
        {
            if (dtos == null || dtos.Count == 0)
                return BadRequest("Request body cannot be empty.");

            try
            {
                var result = await _service.UpdateRangeAsync(dtos, ct);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BagfilterInput batch.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BagfilterInputMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null BagfilterInput.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.BagfilterInputId <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for BagfilterInput.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating BagfilterInput with ID: {BagfilterInputId}", dto.BagfilterInputId);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BagfilterInput with ID: {BagfilterInputId}", dto.BagfilterInputId);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
    }
}
    