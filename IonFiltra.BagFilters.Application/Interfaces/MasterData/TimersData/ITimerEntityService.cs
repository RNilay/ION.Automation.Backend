using IonFiltra.BagFilters.Application.DTOs.MasterData.TimerData;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ITimerEntityService
    {
        Task<TimerEntityMainDto> GetById(int id);
        Task<int> AddAsync(TimerEntityMainDto dto);
        Task UpdateAsync(TimerEntityMainDto dto);

        Task<IEnumerable<TimerEntityMainDto>> GetAll();
    }
}
    