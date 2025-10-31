using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.BagfilterDatabase.WithoutCanopy
{
    [ApiController]
    [Route("api/[controller]")]
    public class IFI_Bagfilter_Database_Without_CanopyController : ControllerBase
    {
        private readonly IIFI_Bagfilter_Database_Without_CanopyService _service;
        private readonly ILogger<IFI_Bagfilter_Database_Without_CanopyController> _logger;

        public IFI_Bagfilter_Database_Without_CanopyController(
            IIFI_Bagfilter_Database_Without_CanopyService service,
            ILogger<IFI_Bagfilter_Database_Without_CanopyController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("get-by-id/{projectId}")]
        public async Task<IActionResult> Get(int projectId)
        {
            _logger.LogInformation("Get started with ProjectId {projectId}", new object[] { projectId });

            try
            {
                var result = await _service.GetByProjectId(projectId);

                if (result == null)
                {
                    _logger.LogWarning("IFI_Bagfilter_Database_Without_Canopy with Project ID: {ProjectId} not found.", new object[] { projectId });
                    return Ok(new
                    {
                        success = false,
                        message = $"IFI_Bagfilter_Database_Without_Canopy with Project ID {projectId} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for ProjectId {ProjectId}", new object[] { projectId });

                return Ok(new
                {
                    success = true,
                    message = "IFI_Bagfilter_Database_Without_Canopy data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching IFI_Bagfilter_Database_Without_Canopy with Project ID: {ProjectId}", new object[] { projectId });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null IFI_Bagfilter_Database_Without_Canopy.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new IFI_Bagfilter_Database_Without_Canopy.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new IFI_Bagfilter_Database_Without_Canopy.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null IFI_Bagfilter_Database_Without_Canopy.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.Id <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for IFI_Bagfilter_Database_Without_Canopy.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating IFI_Bagfilter_Database_Without_Canopy with ID: {Id}", dto.Id);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating IFI_Bagfilter_Database_Without_Canopy with ID: {Id}", dto.Id);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }



        [HttpPost("find-by-match")]
        public async Task<IActionResult> FindByMatch([FromBody] IFI_Bagfilter_Database_Without_Canopy_MatchDto matchDto)
        {
            if (matchDto == null)
            {
                _logger.LogWarning("POST: Received a null match DTO.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("Finding IFI_Bagfilter... by match criteria {@MatchDto}", matchDto);
                var result = await _service.GetByMatchAsync(matchDto.Process_Volume_m3hr, matchDto.Hopper_type, matchDto.Number_of_columns);

                if (result == null)
                {
                    _logger.LogInformation("No matching IFI_Bagfilter found for criteria {@MatchDto}", matchDto);
                    return Ok(new { success = false, message = "No matching record found.", data = (object?)null });
                }

                return Ok(new { success = true, message = "Matching record found.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while finding IFI_Bagfilter by match criteria.");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


    }
}
    