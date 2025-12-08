using IonFiltra.BagFilters.Application.DTOs.MasterData.DPTData;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IDPTEntityService
    {
        Task<DPTMainDto> GetById(int id);
        Task<IEnumerable<DPTMainDto>> GetAll();
        Task<int> AddAsync(DPTMainDto dto);
        Task UpdateAsync(DPTMainDto dto);
        Task DeleteAsync(int id);
    }
}
    