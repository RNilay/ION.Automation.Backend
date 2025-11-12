using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Weight_Summary;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Weight_Summary
{
    public class WeightSummaryService : IWeightSummaryService
    {
        private readonly IWeightSummaryRepository _repository;
        private readonly ILogger<WeightSummaryService> _logger;

        public WeightSummaryService(
            IWeightSummaryRepository repository,
            ILogger<WeightSummaryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<WeightSummaryMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching WeightSummary for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return WeightSummaryMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(WeightSummaryMainDto dto)
        {
            _logger.LogInformation("Adding WeightSummary for Id {Id}", dto.Id);
            var entity = WeightSummaryMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(WeightSummaryMainDto dto)
        {
            _logger.LogInformation("Updating WeightSummary for Id {Id}", dto.Id);
            var entity = WeightSummaryMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    