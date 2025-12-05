using IonFiltra.BagFilters.Application.DTOs.MasterData.SolenoidValveData;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ISolenoidValveService
    {
        Task<SolenoidValveMainDto> GetById(int id);
        Task<int> AddAsync(SolenoidValveMainDto dto);
        Task UpdateAsync(SolenoidValveMainDto dto);

        Task<IEnumerable<SolenoidValveMainDto>> GetAll();

        Task DeleteAsync(int id);
    }
}
    