using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Access_Group;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Access_Group;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Access_Group
{
    public class AccessGroupService : IAccessGroupService
    {
        private readonly IAccessGroupRepository _repository;
        private readonly ILogger<AccessGroupService> _logger;

        public AccessGroupService(
            IAccessGroupRepository repository,
            ILogger<AccessGroupService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AccessGroupMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching AccessGroup for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return AccessGroupMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(AccessGroupMainDto dto)
        {
            _logger.LogInformation("Adding AccessGroup for Id {Id}", dto.Id);
            var entity = AccessGroupMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(AccessGroupMainDto dto)
        {
            _logger.LogInformation("Updating AccessGroup for Id {Id}", dto.Id);
            var entity = AccessGroupMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    