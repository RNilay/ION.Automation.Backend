using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection;

namespace IonFiltra.BagFilters.Application.Interfaces.Bagfilters.Sections.Bag_Selection
{
    public interface IBagSelectionService
    {
        Task<BagSelectionMainDto> GetById(int id);
        Task<int> AddAsync(BagSelectionMainDto dto);
        Task UpdateAsync(BagSelectionMainDto dto);
    }
}
