using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;

namespace IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters
{
    public interface IBagfilterMasterRepository
    {
        Task<BagfilterMaster?> GetByProjectId(int projectId);
        Task<int> AddAsync(BagfilterMaster entity);
        Task UpdateAsync(BagfilterMaster entity);
        Task<List<int>> AddMastersAsync(IEnumerable<BagfilterMaster> masters, CancellationToken ct = default);
    }
}
