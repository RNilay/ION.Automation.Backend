using IonFiltra.BagFilters.Application.DTOs.Users.UserRoles;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IApplicationRolesService
    {
        Task<ApplicationRolesMainDto> GetById(int id);
        Task<List<ApplicationRolesMainDto>> GetAllAsync();
        Task<int> AddAsync(ApplicationRolesMainDto dto);
        Task UpdateAsync(ApplicationRolesMainDto dto);
    }
}
    