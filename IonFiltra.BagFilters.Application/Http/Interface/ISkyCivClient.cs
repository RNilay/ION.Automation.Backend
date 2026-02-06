using Newtonsoft.Json.Linq;

namespace IonFiltra.BagFilters.Infrastructure.Http.Interface
{
    public interface ISkyCivClient
    {
        Task<JObject> PostAsync(object payload, CancellationToken ct = default);

        Task<JObject> PostBOMAsync(object payload, CancellationToken ct = default);
    }

}
