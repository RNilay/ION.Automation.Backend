using IonFiltra.BagFilters.Application.DTOs.BOM.PaintingRates;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.PaintingRates;
using IonFiltra.BagFilters.Application.Mappers.BOM.Rates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.PaintingRates;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.PaintingRates
{
    public class PaintingCostConfigService : IPaintingCostConfigService
    {
        private readonly IPaintingCostConfigRepository _repository;
        private readonly ILogger<PaintingCostConfigService> _logger;

        public PaintingCostConfigService(
            IPaintingCostConfigRepository repository,
            ILogger<PaintingCostConfigService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PaintingCostConfigMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching PaintingCostConfig for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return PaintingCostConfigMapper.ToMainDto(entity);
        }

        public async Task<IEnumerable<PaintingCostConfigMainDto>> GetAll()
        {
            _logger.LogInformation("Fetching all PaintingCostConfig.");
            var entities = await _repository.GetAll();
            return entities.Select(x => PaintingCostConfigMapper.ToMainDto(x));
        }


        public async Task<int> AddAsync(PaintingCostConfigMainDto dto)
        {
            _logger.LogInformation("Adding PaintingCostConfig for Id {Id}", dto.Id);
            var entity = PaintingCostConfigMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(PaintingCostConfigMainDto dto)
        {
            _logger.LogInformation("Updating PaintingCostConfig for Id {Id}", dto.Id);
            var entity = PaintingCostConfigMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    