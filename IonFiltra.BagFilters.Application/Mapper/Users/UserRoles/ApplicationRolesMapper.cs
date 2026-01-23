using IonFiltra.BagFilters.Application.DTOs.Users.UserRoles;
using IonFiltra.BagFilters.Core.Entities.Users.UserRoles;

namespace IonFiltra.BagFilters.Application.Mappers.Users.UserRoles
{
    public static class ApplicationRolesMapper
    {
        public static ApplicationRolesMainDto ToMainDto(ApplicationRoles entity)
        {
            if (entity == null) return null;
            return new ApplicationRolesMainDto
            {
                RoleId = entity.RoleId,
               
                ApplicationRoles = new ApplicationRolesDto
                {
                    RoleName = entity.RoleName,
                    Description = entity.Description,
                    IsActive = entity.IsActive,
                    IsDeleted = entity.IsDeleted,
                },

            };
        }

        public static ApplicationRoles ToEntity(ApplicationRolesMainDto dto)
        {
            if (dto == null) return null;
            return new ApplicationRoles
            {
                RoleId = dto.RoleId,
                
                RoleName = dto.ApplicationRoles.RoleName,
                Description = dto.ApplicationRoles.Description,
                IsActive = dto.ApplicationRoles.IsActive,
                IsDeleted = dto.ApplicationRoles.IsDeleted,

            };
        }
    }
}
