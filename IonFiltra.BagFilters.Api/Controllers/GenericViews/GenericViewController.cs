
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

        [HttpPost("{tableName}")]
        public async Task<IActionResult> Insert(string tableName, [FromBody] Dictionary<string, object> data)
        {
            var id = await _viewService.InsertAsync(tableName, data);
            return Ok(new { id });
        }

        [HttpPut("{tableName}/{id}")]
        public async Task<IActionResult> Update(string tableName, int id, [FromBody] Dictionary<string, object> data)
        {
            await _viewService.UpdateAsync(tableName, id, data);
            return Ok();
        }

        [HttpDelete("{tableName}/{id}")]
        public async Task<IActionResult> Delete(string tableName, int id)
        {
            await _viewService.DeleteAsync(tableName, id);
            return Ok();
        }

        [HttpPut("filterbag/set-default/{id}")]
        public async Task<IActionResult> SetFilterBagDefault(int id)
        {
            try
            {
                // Step 1 — Reset all defaults
                await _viewService.ExecuteRawSqlAsync("UPDATE FilterBag SET IsDefault = 0");

                // Step 2 — Set selected row
                await _viewService.ExecuteRawSqlAsync($"UPDATE FilterBag SET IsDefault = 1 WHERE Id = {id}");

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
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