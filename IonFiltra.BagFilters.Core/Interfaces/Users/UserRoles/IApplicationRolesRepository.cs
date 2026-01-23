using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Users.UserRoles
{
    public interface IApplicationRolesRepository
    {
        Task<ApplicationRoles?> GetById(int id);
        Task<List<ApplicationRoles>> GetAllAsync();
        Task<int> AddAsync(ApplicationRoles entity);
        Task UpdateAsync(ApplicationRoles entity);
    }
}
    