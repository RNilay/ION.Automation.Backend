using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ISupportStructureService
    {
        Task<SupportStructureMainDto> GetById(int id);
        Task<int> AddAsync(SupportStructureMainDto dto);
        Task UpdateAsync(SupportStructureMainDto dto);
    }
}
    