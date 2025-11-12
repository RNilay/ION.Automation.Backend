using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Access_Group;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group
{
    public interface IAccessGroupRepository
    {
        Task<AccessGroup?> GetById(int id);
        Task<int> AddAsync(AccessGroup entity);
        Task UpdateAsync(AccessGroup entity);
    }
}
    