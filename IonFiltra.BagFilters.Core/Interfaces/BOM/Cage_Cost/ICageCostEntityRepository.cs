using IonFiltra.BagFilters.Core.Entities.BOM.Cage_Cost;


namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Cage_Cost
{
    public interface ICageCostEntityRepository
    {
        Task<CageCostEntity?> GetById(int id);
        Task<List<CageCostEntity>> GetByEnquiryId(int enquiryId);
        Task<int> AddAsync(CageCostEntity entity);
        Task UpdateAsync(CageCostEntity entity);

        Task ReplaceForMastersAsync(
        Dictionary<int, List<CageCostEntity>> data,
        CancellationToken ct);
    }
}
    