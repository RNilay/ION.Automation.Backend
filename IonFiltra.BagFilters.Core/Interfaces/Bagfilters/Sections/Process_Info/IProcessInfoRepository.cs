using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info
{
    public interface IProcessInfoRepository
    {
        Task<ProcessInfo?> GetById(int id);
        Task<int> AddAsync(ProcessInfo entity);
        Task UpdateAsync(ProcessInfo entity);
        Task<int?> GetIdForMasterAsync(int bagfilterMasterId, CancellationToken ct = default);

        Task<Dictionary<int, ProcessInfo>> GetByMasterIdsAsync(
        IEnumerable<int> bagfilterMasterIds,
        CancellationToken ct = default);

        Task UpsertRangeAsync(
            IEnumerable<ProcessInfo> entities,
            CancellationToken ct = default);
    }
}
    