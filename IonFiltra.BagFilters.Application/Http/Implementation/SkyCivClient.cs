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
                    ""sections"": ""25.0x25.0x2.6"",
                    ""max_UR_ratio"": 0.4411548915042883
                  }
                ],
                ""msg"": ""Optimizer successfully ran"",
                ""status"": 0,
                ""function"": ""S3D.design.member.optimize"",
                ""last_session_id"": ""21CmUAM9ayBMJahBE35MyTeVQlstFAmFBYgUngnautbMp5FfTQqinoPE1khgm3Ji_1"",
                ""monthly_api_credits"": {
                  ""quota"": 300,
                  ""total_used"": ""43.00000110"",
                  ""used_this_call"": 11
                }
              },
              ""keep_session_open"": true
            }";

            var json = JObject.Parse(text);
            return json;  // ✅ return JObject, not Task<JObject>

        }

    }
}
