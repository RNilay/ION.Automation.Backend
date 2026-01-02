using IonFiltra.BagFilters.Core.Entities.BOM.Transp_Cost;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Transp_Cost
{
    public interface ITransportationCostEntityRepository
    {
        Task<TransportationCostEntity?> GetById(int id);
        Task<int> AddAsync(TransportationCostEntity entity);
        Task UpdateAsync(TransportationCostEntity entity);

        Task ReplaceForMastersAsync(
        Dictionary<int, List<TransportationCostEntity>> data,
        CancellationToken ct);
    }
}
    