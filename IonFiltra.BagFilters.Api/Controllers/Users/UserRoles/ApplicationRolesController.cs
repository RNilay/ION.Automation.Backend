using IonFiltra.BagFilters.Application.DTOs.Users.UserRoles;
using IonFiltra.BagFilters.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IonFiltra.BagFilters.API.Controllers.Users.UserRoles
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationRolesController : ControllerBase
    {
        private readonly IApplicationRolesService _service;
        private readonly ILogger<ApplicationRolesController> _logger;

        public ApplicationRolesController(
            IApplicationRolesService service,
            ILogger<ApplicationRolesController> logger)
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
                    _logger.LogWarning("ApplicationRoles with Project ID: {Id} not found.", new object[] { id });
                    return Ok(new
                    {
                        success = false,
                        message = $"ApplicationRoles with Project ID {id} was not found.",
                        data = (object?)null,
                    });
                }

                _logger.LogInformation("Get completed successfully for Id {Id}", new object[] { id });

                return Ok(new
                {
                    success = true,
                    message = "ApplicationRoles data fetched successfully.",
                    data = result,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ApplicationRoles with Project ID: {Id}", new object[] { id });
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }

        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll started");

            try
            {
                var result = await _service.GetAllAsync();

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("No ApplicationRoles found");
                    return Ok(new
                    {
                        success = false,
                        message = "No ApplicationRoles found.",
                        data = (object?)null
                    });
                }

                _logger.LogInformation("GetAll completed successfully");

                return Ok(new
                {
                    success = true,
                    message = "ApplicationRoles data fetched successfully.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching ApplicationRoles list");
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        }



        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ApplicationRolesMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("POST: Received a null ApplicationRoles.");
                return BadRequest("Request body cannot be null.");
            }

            try
            {
                _logger.LogInformation("POST: Adding new ApplicationRoles.");
                var newId = await _service.AddAsync(dto);
                return StatusCode(201, new { id = newId});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new ApplicationRoles.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
      /*  [HttpGet("get-registration-roles")] // 1. New distinct route
        public async Task<IActionResult> GetRegistrationRoles()
        {
            _logger.LogInformation("GetRegistrationRoles started");

            try
            {
                // Fetch all roles from the service
                var allRoles = await _service.GetAllAsync();

                if (allRoles == null || !allRoles.Any())
                {
                    _logger.LogWarning("No ApplicationRoles found");
                    return Ok(new
                    {
                        success = false,
                        message = "No roles available.",
                        data = (object?)null
                    });
                }

                // 2. SECURITY FILTER: This happens on the SERVER.
                // The browser never receives the "Admin" or "Super Admin" data.
                var publicRoles = allRoles
                    .Where(x => x.ApplicationRoles != null &&
                                x.ApplicationRoles.RoleName == "Purchase / Sales") // Filter Logic
                    .ToList();

                _logger.LogInformation("GetRegistrationRoles completed successfully");

                return Ok(new
                {
                    success = true,
                    message = "Public roles fetched successfully.",
                    data = publicRoles // 3. Return only the safe list
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching registration roles");
                return StatusCode(500, new
                {
                    success = false,
                    data = (object?)null,
                    message = "An error occurred while processing your request."
                });
            }
        } */


        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ApplicationRolesMainDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("PUT: Received a null ApplicationRoles.");
                return BadRequest("Request body cannot be null.");
            }

            if (dto.RoleId <= 0)
            {
                _logger.LogWarning("PUT: Invalid ID for ApplicationRoles.");
                return BadRequest("Invalid ID in request body.");
            }

            try
            {
                _logger.LogInformation("PUT: Updating ApplicationRoles with ID: {RoleId}", dto.RoleId);
                await _service.UpdateAsync(dto);
                return Ok(new {message = "Record updated successfully."});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating ApplicationRoles with ID: {RoleId}", dto.RoleId);
                return StatusCode(500, new {message = "An error occurred while updating the record."});
            }
        }
    }
}
    