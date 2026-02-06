using IonFiltra.BagFilters.Application.Interfaces.BoughtOutItems;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.BoughtOutItems
{
    [ApiController]
    [Route("api/secondary-bought-out-items")]
    public class SecondaryBoughtOutItemController : ControllerBase
    {
        private readonly ISecondaryBoughtOutItemService _service;
        private readonly ILogger<SecondaryBoughtOutItemController> _logger;

        public SecondaryBoughtOutItemController(
            ISecondaryBoughtOutItemService service,
            ILogger<SecondaryBoughtOutItemController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Returns all SecondaryBoughtOutItems for a given enquiry.
        /// </summary>
        [HttpGet("by-enquiry/{enquiryId:int}")]
        public async Task<IActionResult> GetByEnquiry(int enquiryId)
        {
            _logger.LogInformation(
                "GET /api/secondary-bought-out-items/by-enquiry/{EnquiryId}",
                enquiryId);

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
                    message = "Secondary bought-out items fetched successfully.",
                    data = list.Select(x => new
                    {
                        x.Id,
                        x.EnquiryId,
                        x.BagfilterMasterId,
                        x.MasterKey,
                        x.Make,
                        x.Cost,
                        x.Qty,
                        x.Rate,
                        x.Unit
                    })
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error fetching SecondaryBoughtOutItems for enquiry {EnquiryId}",
                    enquiryId);

                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while fetching secondary bought-out items.",
                    data = (object?)null
                });
            }
        }
    }

}
