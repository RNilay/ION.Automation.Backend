using IonFiltra.BagFilters.Application.DTOs.MasterData.DPTData;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.MasterData.DPTData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.DPTData;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.DPTData
{
    public class DPTEntityService : IDPTEntityService
    {
        private readonly IDPTEntityRepository _repository;
        private readonly ILogger<DPTEntityService> _logger;

        public DPTEntityService(
            IDPTEntityRepository repository,
            ILogger<DPTEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DPTMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching DPTEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return DPTEntityMapper.ToMainDto(entity);
        }
        
        public async Task<IEnumerable<DPTMainDto>> GetAll()
        {
            _logger.LogInformation("Fetching all DPTEntity.");

            var entities = await _repository.GetAll();

            return entities.Select(x => DPTEntityMapper.ToMainDto(x));
        }

        public async Task<int> AddAsync(DPTMainDto dto)
        {
            _logger.LogInformation("Adding DPTEntity for Id {Id}", dto.Id);
            var entity = DPTEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(DPTMainDto dto)
        {
            _logger.LogInformation("Updating DPTEntity for Id {Id}", dto.Id);
            var entity = DPTEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
        
        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }
    }
}
    