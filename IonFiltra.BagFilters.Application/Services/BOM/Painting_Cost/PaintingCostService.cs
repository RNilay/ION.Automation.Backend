using IonFiltra.BagFilters.Application.DTOs.BOM.Painting_Cost;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Painting_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Painting_Cost;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Painting_Cost
{
    public class PaintingCostService : IPaintingCostService
    {
        private readonly IPaintingCostRepository _repository;
        private readonly ILogger<PaintingCostService> _logger;

        public PaintingCostService(
            IPaintingCostRepository repository,
            ILogger<PaintingCostService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PaintingCostMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching PaintingCost for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return PaintingCostMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(PaintingCostMainDto dto)
        {
            _logger.LogInformation("Adding PaintingCost for Id {Id}", dto.Id);
            var entity = PaintingCostMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(PaintingCostMainDto dto)
        {
            _logger.LogInformation("Updating PaintingCost for Id {Id}", dto.Id);
            var entity = PaintingCostMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    