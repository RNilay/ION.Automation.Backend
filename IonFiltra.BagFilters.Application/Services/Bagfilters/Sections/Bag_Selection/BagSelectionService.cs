using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Bag_Selection;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Bag_Selection;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Bag_Selection
{
    public class BagSelectionService : IBagSelectionService
    {
        private readonly IBagSelectionRepository _repository;
        private readonly ILogger<BagSelectionService> _logger;

        public BagSelectionService(
            IBagSelectionRepository repository,
            ILogger<BagSelectionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BagSelectionMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching BagSelection for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return BagSelectionMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(BagSelectionMainDto dto)
        {
            _logger.LogInformation("Adding BagSelection for Id {Id}", dto.Id);
            var entity = BagSelectionMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(BagSelectionMainDto dto)
        {
            _logger.LogInformation("Updating BagSelection for Id {Id}", dto.Id);
            var entity = BagSelectionMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    