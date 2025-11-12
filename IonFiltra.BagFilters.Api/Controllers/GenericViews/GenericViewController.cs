
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using IonFiltra.BagFilters.Application.Interfaces.GenericView;

namespace IonFiltra.BagFilters.Api.Controllers.GenericViews
{
    /// <summary>
    /// Controller class for GenericViewController.
    /// </summary>
    [ApiController]
    [Route("api/views")]
    public class GenericViewController : ControllerBase
    {
        private readonly IGenericViewService _viewService;
        private readonly ILogger<GenericViewController> _logger;

        public GenericViewController(IGenericViewService viewService, ILogger<GenericViewController> logger)
        {
            _viewService = viewService;
            _logger = logger;
        }

        /// <summary>
        /// Method to fetch the GetViewData method by View Name.
        /// </summary>
        /// <param name="viewName">Project Id</param>
        [HttpGet("{viewName}")]
        public async Task<IActionResult> GetViewData(string viewName)
        {
           _logger.LogInformation("GetViewData started with Viewname {viewName}", new object[] { viewName });
            var data = await _viewService.GetViewData(viewName);
            return Ok(data);
        }
        /// <summary>
        /// Method to fetch the GetViewDataWithParams.
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="Dictionary<string"></param>
        /// <param name="parameters"></param>
        [HttpPost("with-params/{viewName}")]
        public async Task<IActionResult> GetViewDataWithParams(string viewName, [FromBody] Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return BadRequest("Parameters cannot be null");

            // Convert JsonElement values to C# types
            var processedParams = parameters.ToDictionary(
                kvp => kvp.Key,
                kvp => ConvertJsonElement(kvp.Value)
            );

            // Proper structured logging
            _logger.LogInformation(
                "GetViewDataWithParams started with ViewName {viewName} and Parameters {@parameters}",
                viewName,
                processedParams);

            var data = await _viewService.GetViewDataWithParam(viewName, processedParams);
            return Ok(data);
        }


        // 🚀 Helper function to convert JsonElement values
        private static object ConvertJsonElement(object value)
        {
            if (value is JsonElement jsonElement)
            {
                return jsonElement.ValueKind switch
                {
                    JsonValueKind.String => jsonElement.GetString(),
                    JsonValueKind.Number => jsonElement.TryGetInt64(out long l) ? l : jsonElement.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => null // Handle nulls or unsupported types
                };
            }
            return value;
        }


    }
}