using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IStructureInputsService
    {
        Task<StructureInputsMainDto> GetById(int id);
        Task<int> AddAsync(StructureInputsMainDto dto);
        Task UpdateAsync(StructureInputsMainDto dto);
    }
}
    