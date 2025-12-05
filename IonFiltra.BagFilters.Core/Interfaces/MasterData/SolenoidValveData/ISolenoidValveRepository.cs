using IonFiltra.BagFilters.Core.Entities.MasterData.SolenoidValveData;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.SolenoidValveData
{
    public interface ISolenoidValveRepository
    {
        Task<SolenoidValve?> GetById(int id);
        Task<int> AddAsync(SolenoidValve entity);
        Task UpdateAsync(SolenoidValve entity);

        Task<IEnumerable<SolenoidValve>> GetAll();

        Task SoftDeleteAsync(int id);
    }
}
    