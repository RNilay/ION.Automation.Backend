using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ICageInputsService
    {
        Task<CageInputsMainDto> GetById(int id);
        Task<int> AddAsync(CageInputsMainDto dto);
        Task UpdateAsync(CageInputsMainDto dto);
    }
}
    