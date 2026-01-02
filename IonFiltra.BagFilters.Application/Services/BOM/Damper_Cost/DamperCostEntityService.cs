using IonFiltra.BagFilters.Application.DTOs.BOM.Damper_Cost;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Damper_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Damper_Cost;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Damper_Cost
{
    public class DamperCostEntityService : IDamperCostEntityService
    {
        private readonly IDamperCostEntityRepository _repository;
        private readonly ILogger<DamperCostEntityService> _logger;

        public DamperCostEntityService(
            IDamperCostEntityRepository repository,
            ILogger<DamperCostEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DamperCostMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching DamperCostEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return DamperCostEntityMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(DamperCostMainDto dto)
        {
            _logger.LogInformation("Adding DamperCostEntity for Id {Id}", dto.Id);
            var entity = DamperCostEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(DamperCostMainDto dto)
        {
            _logger.LogInformation("Updating DamperCostEntity for Id {Id}", dto.Id);
            var entity = DamperCostEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    