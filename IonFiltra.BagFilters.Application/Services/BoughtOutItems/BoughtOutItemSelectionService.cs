using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.BoughtOutItems;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.BoughtOutItems
{
    public class BoughtOutItemSelectionService : IBoughtOutItemSelectionService
    {
        private readonly IBoughtOutItemSelectionRepository _repository;
        private readonly ILogger<BoughtOutItemSelectionService> _logger;

        public BoughtOutItemSelectionService(
            IBoughtOutItemSelectionRepository repository,
            ILogger<BoughtOutItemSelectionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BoughtOutItemSelectionMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching BoughtOutItemSelection for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return BoughtOutItemSelectionMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(BoughtOutItemSelectionMainDto dto)
        {
            _logger.LogInformation("Adding BoughtOutItemSelection for Id {Id}", dto.Id);
            var entity = BoughtOutItemSelectionMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(BoughtOutItemSelectionMainDto dto)
        {
            _logger.LogInformation("Updating BoughtOutItemSelection for Id {Id}", dto.Id);
            var entity = BoughtOutItemSelectionMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public Task<List<BoughtOutItemSelection>> GetByEnquiryAsync(int enquiryId)
        {
            if (enquiryId <= 0)
                throw new ArgumentOutOfRangeException(nameof(enquiryId));

            _logger.LogInformation("Service: fetching BoughtOutItemSelection for EnquiryId {EnquiryId}", enquiryId);
            return _repository.GetByEnquiryAsync(enquiryId);
        }
    }
}
    