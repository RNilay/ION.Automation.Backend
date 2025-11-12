using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ICapsuleInputsService
    {
        Task<CapsuleInputsMainDto> GetById(int id);
        Task<int> AddAsync(CapsuleInputsMainDto dto);
        Task UpdateAsync(CapsuleInputsMainDto dto);
    }
}
    