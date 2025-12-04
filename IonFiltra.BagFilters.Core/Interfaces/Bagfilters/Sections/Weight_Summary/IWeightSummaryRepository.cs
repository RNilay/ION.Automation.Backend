using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary
{
    public interface IWeightSummaryRepository
    {
        Task<WeightSummary?> GetById(int id);
        Task<int> AddAsync(WeightSummary entity);
        Task UpdateAsync(WeightSummary entity);

        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);




        Task<Dictionary<int, WeightSummary>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<WeightSummary> entities,
            CancellationToken ct = default);

    }
}
    