using IonFiltra.BagFilters.Application.DTOs.Users.UserRoles;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Users.UserRoles;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Users.UserRoles;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Users.UserRoles
{
    public class ApplicationRolesService : IApplicationRolesService
    {
        private readonly IApplicationRolesRepository _repository;
        private readonly ILogger<ApplicationRolesService> _logger;

        public ApplicationRolesService(
            IApplicationRolesRepository repository,
            ILogger<ApplicationRolesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ApplicationRolesMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching ApplicationRoles for Id {RoleId}", id);
            var entity = await _repository.GetById(id);
            return ApplicationRolesMapper.ToMainDto(entity);
        }

        public async Task<List<ApplicationRolesMainDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all ApplicationRoles");

            var entities = await _repository.GetAllAsync();

            // Assuming mapper supports list mapping; if not, map in loop
            return entities
                .Select(ApplicationRolesMapper.ToMainDto)
                .ToList();
        }


        public async Task<int> AddAsync(ApplicationRolesMainDto dto)
        {
            _logger.LogInformation("Adding ApplicationRoles for Id {RoleId}", dto.RoleId);
            var entity = ApplicationRolesMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.RoleId;
        }

        public async Task UpdateAsync(ApplicationRolesMainDto dto)
        {
            _logger.LogInformation("Updating ApplicationRoles for Id {RoleId}", dto.RoleId);
            var entity = ApplicationRolesMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    