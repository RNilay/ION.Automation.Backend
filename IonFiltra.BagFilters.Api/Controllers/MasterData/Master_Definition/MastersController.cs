using IonFiltra.BagFilters.Application.Interfaces.MasterData.Master_Definition;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.Api.Controllers.MasterData.Master_Definition
{
    [ApiController]
    [Route("api/masters")]
    public class MastersController : ControllerBase
    {
        private readonly IMasterDefinitionsService _service;
        private readonly ILogger<MastersController> _logger;

        public MastersController(
            IMasterDefinitionsService service,
            ILogger<MastersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Returns all active master definitions (metadata) for the frontend.
        /// </summary>
        [HttpGet("metadata")]
        public async Task<IActionResult> GetMetadata()
        {
            _logger.LogInformation("GET /api/masters/metadata started.");

            try
            {
                var result = await _service.GetAllActiveAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No active MasterDefinitions found.");
                    return Ok(new
                    {
                        success = false,
                        message = "No master definitions found.",
                        data = (object?)null
                    });
                }

                _logger.LogInformation("MasterDefinitions metadata fetched successfully.");

                return Ok(new
                {
                    success = true,
                    message = "MasterDefinitions metadata fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching MasterDefinitions metadata.");
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
