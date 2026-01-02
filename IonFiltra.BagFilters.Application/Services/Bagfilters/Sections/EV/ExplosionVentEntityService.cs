using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.EV;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.EV;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.EV
{
    public class ExplosionVentEntityService : IExplosionVentEntityService
    {
        private readonly IExplosionVentEntityRepository _repository;
        private readonly ILogger<ExplosionVentEntityService> _logger;

        public ExplosionVentEntityService(
            IExplosionVentEntityRepository repository,
            ILogger<ExplosionVentEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ExplosionVentEntityMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching ExplosionVentEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return ExplosionVentEntityMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(ExplosionVentEntityMainDto dto)
        {
            _logger.LogInformation("Adding ExplosionVentEntity for Id {Id}", dto.Id);
            var entity = ExplosionVentEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(ExplosionVentEntityMainDto dto)
        {
            _logger.LogInformation("Updating ExplosionVentEntity for Id {Id}", dto.Id);
            var entity = ExplosionVentEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    