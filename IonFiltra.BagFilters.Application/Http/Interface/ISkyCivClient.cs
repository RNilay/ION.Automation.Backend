using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Infrastructure.Http.Interface
{
    public interface ISkyCivClient
    {
        Task<JObject> PostForAnalysisAsync(object payload, CancellationToken ct = default);
    }

}
