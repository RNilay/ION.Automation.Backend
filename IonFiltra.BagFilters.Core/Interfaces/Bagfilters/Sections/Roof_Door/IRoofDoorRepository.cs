using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Roof_Door;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door
{
    public interface IRoofDoorRepository
    {
        Task<RoofDoor?> GetById(int id);
        Task<int> AddAsync(RoofDoor entity);
        Task UpdateAsync(RoofDoor entity);
    }
}
    