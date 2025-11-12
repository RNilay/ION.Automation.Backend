using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Hopper_Trough;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IHopperInputsService
    {
        Task<HopperInputsMainDto> GetById(int id);
        Task<int> AddAsync(HopperInputsMainDto dto);
        Task UpdateAsync(HopperInputsMainDto dto);
    }
}
    