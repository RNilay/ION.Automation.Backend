using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.MasterData.BoughtOutItems
{
    [ApiController]
    [Route("api/bought-out-items")]
    public class BoughtOutItemSelectionController : ControllerBase
    {
        private readonly IBoughtOutItemSelectionService _service;
        private readonly ILogger<BoughtOutItemSelectionController> _logger;

        public BoughtOutItemSelectionController(
            IBoughtOutItemSelectionService service,
            ILogger<BoughtOutItemSelectionController> logger)
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
                    _logger.LogWarning("BoughtOutItemSelection with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"BoughtOutItemSelection with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "BoughtOutItemSelection data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching BoughtOutItemSelection with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] BoughtOutItemSelectionMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null BoughtOutItemSelection.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new BoughtOutItemSelection.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new BoughtOutItemSelection.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] BoughtOutItemSelectionMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null BoughtOutItemSelection.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for BoughtOutItemSelection.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating BoughtOutItemSelection with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating BoughtOutItemSelection with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }



        /// <summary>
        /// Returns all BoughtOutItemSelection rows for a given enquiry.
        /// </summary>
        [HttpGet("by-enquiry/{enquiryId:int}")]
        public async Task<IActionResult> GetByEnquiry(int enquiryId)
        {
            _logger.LogInformation("GET /api/bought-out-items/by-enquiry/{EnquiryId} started", enquiryId);

            if (enquiryId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid enquiry id.",
                    data = (object?)null
                });
            }

            try
            {
                var list = await _service.GetByEnquiryAsync(enquiryId);

                return Ok(new
                {
                    success = true,
                    message = "Bought out items fetched successfully.",
                    data = list.Select(x => new
                    {
                        x.Id,
                        x.EnquiryId,
                        x.BagfilterMasterId,
                        x.MasterDefinitionId,
                        x.MasterKey,
                        x.SelectedRowId
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching BoughtOutItemSelection for enquiry {EnquiryId}", enquiryId);
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching bought-out items.",
                    data = (object?)null
                });
            }
        }


    }
}
    