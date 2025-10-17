using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.SkyCiv;
using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Core.Interfaces.SkyCiv;
using IonFiltra.BagFilters.Infrastructure.Http.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using IonFiltra.BagFilters.Core.Entities;
using IonFiltra.BagFilters.Application.DTOs;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Core.Entities.SkyCivEntities; // if SkyCivOptions placed here
// plus any repo namespaces

namespace IonFiltra.BagFilters.Application.Services.SkyCiv
{
    public class SkyCivAnalysisService : ISkyCivAnalysisService
    {
        private readonly ISkyCivClient _client;
        private readonly IAnalysisSessionRepository _repo;
        private readonly ILogger<SkyCivAnalysisService> _logger;
        private readonly SkyCivOptions _opts;

        public SkyCivAnalysisService(
            ISkyCivClient client,
            IAnalysisSessionRepository repo,
            IOptions<SkyCivOptions> opts,
            ILogger<SkyCivAnalysisService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (opts == null) throw new ArgumentNullException(nameof(opts));
            _opts = opts.Value;
        }

        public async Task<AnalysisResponseDto> RunAnalysisAsync(JObject s3dModel, CancellationToken ct)
        {
            if (s3dModel == null) throw new ArgumentNullException(nameof(s3dModel));

            // 1. Create session record
            var session = new AnalysisSession
            {
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            //await _repo.AddAsync(session);

            // 2. Build payload
            var payload = BuildPayload(s3dModel, _opts.Username, _opts.Key);

            // 3. Call SkyCiv
            JObject result;
            try
            {
                // Note: PostForAnalysisAsync expected signature: Task<JObject> PostForAnalysisAsync(object payload, CancellationToken ct = default)
                result = await _client.PostForAnalysisAsync(payload, ct);
            }
            catch (OperationCanceledException oce) when (ct.IsCancellationRequested)
            {
                _logger.LogWarning(oce, "SkyCiv analysis cancelled by caller.");
                session.Status = "Cancelled";
                session.ErrorMessage = "Cancelled";
                //await _repo.UpdateAsync(session);
                return new AnalysisResponseDto { Status = "Cancelled", Message = "Operation cancelled" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling SkyCiv API.");
                session.Status = "Failed";
                session.ErrorMessage = ex.Message;
                //await _repo.UpdateAsync(session);
                return new AnalysisResponseDto { Status = "Failed", Message = ex.Message };
            }

            // 4. Parse functions -> extract session id and model.get data
            var sessionId = ExtractSessionId(result);
            var modelDataToken = ExtractModelGetData(result);

            session.SessionId = sessionId;
            session.Status = "Succeeded";
            //await _repo.UpdateAsync(session);

            // 5. Persist result
            var analysisResult = new AnalysisResult
            {
                AnalysisSessionId = session.Id,
                ModelGetDataJson = modelDataToken?.ToString(Formatting.None),
                FullResponseJson = result?.ToString(Formatting.None),
                SavedAt = DateTime.UtcNow
            };
            //await _repo.AddResultAsync(analysisResult);

            return new AnalysisResponseDto
            {
                SessionId = sessionId,
                ModelData = modelDataToken as JObject, // returns null if not an object
                Status = "Succeeded"
            };
        }

        #region Helpers (private)

        // Build payload matching your JS payload shape but using provided username/key
        private object BuildPayload(JObject s3dModel, string username, string key)
        {
            // You can build a strongly typed DTO instead of object; keeping it dynamic/object for brevity.
            var plateIds = s3dModel["plates"] != null
                ? s3dModel["plates"].ToObject<Dictionary<string, object>>().Keys.Select(k => int.Parse(k)).ToArray()
                : Array.Empty<int>();

            var loadCombinations = s3dModel["load_combinations"] != null
                ? s3dModel["load_combinations"].ToObject<Dictionary<string, object>>().Keys.Select(k => int.Parse(k)).ToArray()
                : Array.Empty<int>();

            var payload = new
            {
                auth = new { username = username, key = key, source = "Backend" },
                functions = new object[]
                {
                    new { function = "S3D.session.start", arguments = new { keep_open = true } },
                    new { function = "S3D.model.set", arguments = new { s3d_model = s3dModel } },
                    new { function = "S3D.model.mesh", arguments = new { method = "delaunay", granularity = 1, plate_ids = plateIds } },
                    new { function = "S3D.model.solve", arguments = new { analysis_type = "linear", repair_model = true, return_data = true, format = "json" } },
                    new { function = "S3D.model.get" },
                    new { function = "S3D.results.get" },
                    new { function = "S3D.results.getAnalysisReport", arguments = new { job_name = "API Job", file_type = "txt", load_combinations = loadCombinations } }
                },
                sandbox = false
            };

            return payload;
        }

        // Extracts last_session_id from the response functions (null if not found)
        private string ExtractSessionId(JObject response)
        {
            if (response == null) return null;

            var funcs = response["functions"] as JArray;
            if (funcs == null) return null;

            foreach (var f in funcs)
            {
                var funcName = f["function"]?.ToString();
                if (string.Equals(funcName, "S3D.session.start", StringComparison.OrdinalIgnoreCase))
                {
                    var sid = f["session_id"]?.ToString();
                    if (!string.IsNullOrEmpty(sid)) return sid;
                }

                // Some APIs might place session id elsewhere; check top-level fields as fallback
            }

            // fallback to response.response.session_id or last_session_id
            var topLevelSid = response["last_session_id"]?.ToString() ?? response["response"]?["session_id"]?.ToString();
            return topLevelSid;
        }

        // Extracts the data from S3D.model.get (returns JToken so caller can parse/convert)
        private JToken ExtractModelGetData(JObject response)
        {
            if (response == null) return null;

            var funcs = response["functions"] as JArray;
            if (funcs == null) return null;

            foreach (var f in funcs)
            {
                var funcName = f["function"]?.ToString();
                if (string.Equals(funcName, "S3D.model.get", StringComparison.OrdinalIgnoreCase))
                {
                    // function may contain 'data' property
                    var data = f["data"];
                    if (data != null) return data;
                }

                // Some APIs might return the modified input in different functions,
                // check for model.solve returning data as fallback:
                if (string.Equals(funcName, "S3D.model.solve", StringComparison.OrdinalIgnoreCase))
                {
                    var data = f["data"];
                    if (data != null) return data;
                }
            }

            // fallback: some APIs include 'response' -> 'data' or 'result' top-level
            var fallback = response["response"]?["data"];
            return fallback;
        }

        #endregion
    }
}
