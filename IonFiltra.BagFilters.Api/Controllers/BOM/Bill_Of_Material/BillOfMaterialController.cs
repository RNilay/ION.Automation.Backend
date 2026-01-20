using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.BOM.Bill_Of_Material
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillOfMaterialController : ControllerBase
    {
        private readonly IBillOfMaterialService _service;
        private readonly ILogger<BillOfMaterialController> _logger;

        public BillOfMaterialController(
            IBillOfMaterialService service,
            ILogger<BillOfMaterialController> logger)
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
                    _logger.LogWarning("BillOfMaterial with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"BillOfMaterial with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "BillOfMaterial data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching BillOfMaterial with Project ID: {Id}", new object[] { id });
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
            _logger.LogInformation(
                "Get BillOfMaterial list started for EnquiryId {EnquiryId}",
                enquiryId);

            try
            {
                var result = await _service.GetByEnquiryIdAsync(enquiryId);

                // IMPORTANT: list being empty is NOT an error
                if (result == null || result.Count == 0)
                {
                    _logger.LogWarning(
                        "No BillOfMaterial records found for EnquiryId {EnquiryId}",
                        enquiryId);

                    return Ok(new
                    {
                        success = true,
                        message = $"No BillOfMaterial records found for EnquiryId {enquiryId}.",
                        data = new List<object>()
                    });
                }

                _logger.LogInformation(
                    "Get BillOfMaterial list completed successfully for EnquiryId {EnquiryId}",
                    enquiryId);

                return Ok(new
                {
                    success = true,
                    message = "BillOfMaterial list fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while fetching BillOfMaterial list for EnquiryId {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing your request.",
                    data = (object?)null
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BillOfMaterialMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null BillOfMaterial.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new BillOfMaterial.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new BillOfMaterial.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BillOfMaterialMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null BillOfMaterial.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for BillOfMaterial.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating BillOfMaterial with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BillOfMaterial with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
    }
}
    