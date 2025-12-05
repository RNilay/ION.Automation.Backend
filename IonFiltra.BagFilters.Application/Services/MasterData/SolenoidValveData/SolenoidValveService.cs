using IonFiltra.BagFilters.Application.DTOs.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.MasterData.SolenoidValveData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.SolenoidValveData;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.SolenoidValveData
{
    public class SolenoidValveService : ISolenoidValveService
    {
        private readonly ISolenoidValveRepository _repository;
        private readonly ILogger<SolenoidValveService> _logger;

        public SolenoidValveService(
            ISolenoidValveRepository repository,
            ILogger<SolenoidValveService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SolenoidValveMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching SolenoidValve for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return SolenoidValveMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(SolenoidValveMainDto dto)
        {
            _logger.LogInformation("Adding SolenoidValve for Id {Id}", dto.Id);
            var entity = SolenoidValveMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(SolenoidValveMainDto dto)
        {
            _logger.LogInformation("Updating SolenoidValve for Id {Id}", dto.Id);
            var entity = SolenoidValveMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task<IEnumerable<SolenoidValveMainDto>> GetAll()
        {
            _logger.LogInformation("Fetching all SolenoidValves.");

            var entities = await _repository.GetAll();

            return entities.Select(SolenoidValveMapper.ToMainDto);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.SoftDeleteAsync(id);
        }

    }
}
    