using IonFiltra.BagFilters.Core.Entities.MasterData.DPTData;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.DPTData
{
    public interface IDPTEntityRepository
    {
        Task<DPTEntity?> GetById(int id);
        Task<IEnumerable<DPTEntity>> GetAll();
        Task<int> AddAsync(DPTEntity entity);
        Task UpdateAsync(DPTEntity entity);
        Task SoftDeleteAsync(int id);
    }
}
    