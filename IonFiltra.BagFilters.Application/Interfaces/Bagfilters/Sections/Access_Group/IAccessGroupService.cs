using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IAccessGroupService
    {
        Task<AccessGroupMainDto> GetById(int id);
        Task<int> AddAsync(AccessGroupMainDto dto);
        Task UpdateAsync(AccessGroupMainDto dto);
    }
}
    