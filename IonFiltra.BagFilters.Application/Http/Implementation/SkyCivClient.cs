using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Common;
using IonFiltra.BagFilters.Infrastructure.Http.Interface;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace IonFiltra.BagFilters.Infrastructure.Http.Implementation
{
    public class SkyCivClient : ISkyCivClient
    {
        private readonly HttpClient _http;
        private readonly SkyCivOptions _opts;

        public SkyCivClient(HttpClient http, IOptions<SkyCivOptions> opts)
        {
            _http = http;
            _opts = opts.Value;
        }

        //public async Task<JObject> PostForAnalysisAsync(object payload, CancellationToken ct = default)
        //{
        //    var json = JsonConvert.SerializeObject(payload);
        //    using var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    // If you want to add auth to payload instead of header, include it in payload (your payload does that)
        //    using var response = await _http.PostAsync(_opts.ApiUrl, content, ct);
        //    var text = await response.Content.ReadAsStringAsync(ct);

        //    // Optionally, do not call EnsureSuccessStatusCode if the API returns 200 with error payloads.
        //    // response.EnsureSuccessStatusCode();

        //    return JObject.Parse(text);
        //}
        public async Task<JObject> PostForAnalysisAsync(object payload, CancellationToken ct = default)
        {
            var json = JsonConvert.SerializeObject(payload);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            // link caller token + our own timeout
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromMinutes(10)); // cancels after 10 minutes

            // Post to base address (empty relative URI)
            using var response = await _http.PostAsync("", content, cts.Token);
            response.EnsureSuccessStatusCode(); // fail fast on non-2xx

            var text = await response.Content.ReadAsStringAsync(ct);

            return JObject.Parse(text);
        }

    }
}
