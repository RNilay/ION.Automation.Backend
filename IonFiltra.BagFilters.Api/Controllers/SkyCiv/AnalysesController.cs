using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Api.Controllers.SkyCiv
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysesController : ControllerBase
    {
        private readonly ISkyCivAnalysisService _service;
        public AnalysesController(ISkyCivAnalysisService service) => _service = service;

        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody] JObject s3dModel, CancellationToken ct)
        {
            if (s3dModel == null) return BadRequest("s3dModel required");

            var result = await _service.RunAnalysisAsync(s3dModel, ct);
            if (result == null) return StatusCode(500, "Analysis failed");
            var json = JsonConvert.SerializeObject(result);
            return Ok(new { success = true, analysisResult = json });
        }
    }
}
