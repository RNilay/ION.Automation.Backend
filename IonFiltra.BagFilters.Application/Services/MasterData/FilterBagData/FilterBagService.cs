using IonFiltra.BagFilters.Application.DTOs.MasterData.FilterBagData;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.MasterData.FilterBagData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.FilterBagData;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.FilterBagData
{
    public class FilterBagService : IFilterBagService
    {
        private readonly IFilterBagRepository _repository;
        private readonly ILogger<FilterBagService> _logger;

        public FilterBagService(
            IFilterBagRepository repository,
            ILogger<FilterBagService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<FilterBagMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching FilterBag for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return FilterBagMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(FilterBagMainDto dto)
        {
            _logger.LogInformation("Adding FilterBag for Id {Id}", dto.Id);
            var entity = FilterBagMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(FilterBagMainDto dto)
        {
            _logger.LogInformation("Updating FilterBag for Id {Id}", dto.Id);
            var entity = FilterBagMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    