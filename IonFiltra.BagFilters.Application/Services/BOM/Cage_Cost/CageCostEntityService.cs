using IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Cage_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Cage_Cost;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Cage_Cost
{
    public class CageCostEntityService : ICageCostEntityService
    {
        private readonly ICageCostEntityRepository _repository;
        private readonly ILogger<CageCostEntityService> _logger;

        public CageCostEntityService(
            ICageCostEntityRepository repository,
            ILogger<CageCostEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CageCostMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching CageCostEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return CageCostEntityMapper.ToMainDto(entity);
        }

        public async Task<List<CageCostMainDto>> GetByEnquiryId(int enquiryId)
        {
            _logger.LogInformation(
                "Fetching CageCostEntity list for EnquiryId {EnquiryId}",
                enquiryId);

            var entities = await _repository.GetByEnquiryId(enquiryId);

            return entities
                .Select(CageCostEntityMapper.ToMainDto)
                .ToList();
        }


        public async Task<int> AddAsync(CageCostMainDto dto)
        {
            _logger.LogInformation("Adding CageCostEntity for Id {Id}", dto.Id);
            var entity = CageCostEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(CageCostMainDto dto)
        {
            _logger.LogInformation("Updating CageCostEntity for Id {Id}", dto.Id);
            var entity = CageCostEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    