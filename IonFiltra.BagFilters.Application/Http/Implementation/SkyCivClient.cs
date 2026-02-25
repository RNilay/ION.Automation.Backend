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

      
        public async Task<JObject> PostAsync(object payload, CancellationToken ct = default)
        {
            //var json = JsonConvert.SerializeObject(payload);
            //using var content = new StringContent(json, Encoding.UTF8, "application/json");

            //// link caller token + our own timeout
            //using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            //cts.CancelAfter(TimeSpan.FromMinutes(10)); // cancels after 10 minutes

            //// Post to base address (empty relative URI)
            //using var response = await _http.PostAsync("", content, cts.Token);
            //response.EnsureSuccessStatusCode(); // fail fast on non-2xx

            //var text = await response.Content.ReadAsStringAsync(ct);

            //return JObject.Parse(text);


            var text = @"
            {
              ""response"": {
                ""data"": [
                  {
                    ""sections"": ""45.0x45.0x2.6"",
                    ""max_UR_ratio"": 0.8420872694602191
                  },
                  {
                    ""sections"": ""MC75"",
                    ""max_UR_ratio"": 0.21283160690357036
                  },
                    {
                    ""sections"": ""A2020x3"",
                    ""max_UR_ratio"": 0.2166302268908826
                  },
                  {
                    ""sections"": ""45.0x45.0x2.6"",
                    ""max_UR_ratio"": 0.8768011654541732
                  }
                ],
                ""msg"": ""Optimizer successfully ran"",
                ""status"": 0,
                ""function"": ""S3D.design.member.optimize"",
                ""last_session_id"": ""6JTcHzE0BKd81QYxNcre6KeDtfLRsNalpbQVX7LtdrUNnEsLy97aslHn0TWDr4Hi_0"",
                ""monthly_api_credits"": {
                  ""quota"": 300,
                  ""total_used"": ""12.0000013"",
                  ""used_this_call"": 4
                }
              },
              ""keep_session_open"": true
            }";

            var json = JObject.Parse(text);
            return json;  // ✅ return JObject, not Task<JObject>

        }



        public async Task<JObject> PostBOMAsync(object payload, CancellationToken ct = default)
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
