using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.DamperSize;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IDamperSizeInputsService
    {
        Task<DamperSizeInputsMainDto> GetById(int id);
        Task<int> AddAsync(DamperSizeInputsMainDto dto);
        Task UpdateAsync(DamperSizeInputsMainDto dto);
    }
}
    