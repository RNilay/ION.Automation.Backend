using IonFiltra.BagFilters.Application.DTOs.BOM.Rates;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Rates;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Rates;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Rates
{
    public class BillOfMaterialRatesService : IBillOfMaterialRatesService
    {
        private readonly IBillOfMaterialRatesRepository _repository;
        private readonly ILogger<BillOfMaterialRatesService> _logger;

        public BillOfMaterialRatesService(
            IBillOfMaterialRatesRepository repository,
            ILogger<BillOfMaterialRatesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BillOfMaterialRatesMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching BillOfMaterialRates for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return BillOfMaterialRatesMapper.ToMainDto(entity);
        }

        public async Task<IEnumerable<BillOfMaterialRatesMainDto>> GetAll()
        {
            _logger.LogInformation("Fetching all BillOfMaterialRates.");
            var entities = await _repository.GetAll();
            return entities.Select(x => BillOfMaterialRatesMapper.ToMainDto(x));
        }


        public async Task<int> AddAsync(BillOfMaterialRatesMainDto dto)
        {
            _logger.LogInformation("Adding BillOfMaterialRates for Id {Id}", dto.Id);
            var entity = BillOfMaterialRatesMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(BillOfMaterialRatesMainDto dto)
        {
            _logger.LogInformation("Updating BillOfMaterialRates for Id {Id}", dto.Id);
            var entity = BillOfMaterialRatesMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    