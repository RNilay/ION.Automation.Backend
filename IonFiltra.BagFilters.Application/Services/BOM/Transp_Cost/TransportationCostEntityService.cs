using IonFiltra.BagFilters.Application.DTOs.BOM.Transp_Cost;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Transp_Cost;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Transp_Cost;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Transp_Cost
{
    public class TransportationCostEntityService : ITransportationCostEntityService
    {
        private readonly ITransportationCostEntityRepository _repository;
        private readonly ILogger<TransportationCostEntityService> _logger;

        public TransportationCostEntityService(
            ITransportationCostEntityRepository repository,
            ILogger<TransportationCostEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TransportationCostMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching TransportationCostEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return TransportationCostEntityMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(TransportationCostMainDto dto)
        {
            _logger.LogInformation("Adding TransportationCostEntity for Id {Id}", dto.Id);
            var entity = TransportationCostEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(TransportationCostMainDto dto)
        {
            _logger.LogInformation("Updating TransportationCostEntity for Id {Id}", dto.Id);
            var entity = TransportationCostEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    