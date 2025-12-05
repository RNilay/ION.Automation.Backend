using IonFiltra.BagFilters.Core.Entities.MasterData.TimerData;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.TimerData
{
    public interface ITimerEntityRepository
    {
        Task<TimerEntity?> GetById(int id);
        Task<int> AddAsync(TimerEntity entity);
        Task UpdateAsync(TimerEntity entity);

        Task<IEnumerable<TimerEntity>> GetAll();
    }
}
    