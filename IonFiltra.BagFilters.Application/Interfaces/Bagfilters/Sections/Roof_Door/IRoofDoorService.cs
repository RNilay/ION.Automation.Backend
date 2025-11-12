using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Roof_Door;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IRoofDoorService
    {
        Task<RoofDoorMainDto> GetById(int id);
        Task<int> AddAsync(RoofDoorMainDto dto);
        Task UpdateAsync(RoofDoorMainDto dto);
    }
}
    