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
                    ""sections"": ""113.5x113.5x4.8"",
                    ""max_UR_ratio"": 0.9312089753443507
                  },
                  {
                    ""sections"": ""JB150"",
                    ""max_UR_ratio"": 0.00033933812192741777
                  },
                    {
                    ""sections"": ""A8040x5"",
                    ""max_UR_ratio"": 29.32138665029781
                  },
                  {
                    ""sections"": ""NT 40"",
                    ""max_UR_ratio"": 0.24505866882377741
                  }
                ],
                ""msg"": ""Optimizer successfully ran"",
                ""status"": 0,
                ""function"": ""S3D.design.member.optimize"",
                ""last_session_id"": ""n6qpizSHoDWXkfgq0DFxEiOWEnLsRrmF3jI0YuiIamhOi114pICrkLUJl5VA95qc_4"",
                ""monthly_api_credits"": {
                  ""quota"": 300,
                  ""total_used"": ""4.0000013"",
                  ""used_this_call"": 4
                }
              },
              ""keep_session_open"": true
            }";

            var json = JObject.Parse(text);
            return json;  // ✅ return JObject, not Task<JObject>

        }

    }
}
