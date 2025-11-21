using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Painting;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IPaintingAreaService
    {
        Task<PaintingAreaMainDto> GetById(int id);
        Task<int> AddAsync(PaintingAreaMainDto dto);
        Task UpdateAsync(PaintingAreaMainDto dto);
    }
}
    