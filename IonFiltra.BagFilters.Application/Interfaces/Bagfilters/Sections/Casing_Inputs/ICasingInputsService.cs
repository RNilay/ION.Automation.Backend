using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ICasingInputsService
    {
        Task<CasingInputsMainDto> GetById(int id);
        Task<int> AddAsync(CasingInputsMainDto dto);
        Task UpdateAsync(CasingInputsMainDto dto);
    }
}
    