using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Painting;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting
{
    public interface IPaintingAreaRepository
    {
        Task<PaintingArea?> GetById(int id);
        Task<int> AddAsync(PaintingArea entity);
        Task UpdateAsync(PaintingArea entity);
    }
}
    