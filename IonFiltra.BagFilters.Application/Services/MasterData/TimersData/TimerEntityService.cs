using IonFiltra.BagFilters.Application.DTOs.MasterData.TimerData;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.MasterData.TimerData;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.MasterData.TimerData;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.MasterData.TimerData
{
    public class TimerEntityService : ITimerEntityService
    {
        private readonly ITimerEntityRepository _repository;
        private readonly ILogger<TimerEntityService> _logger;

        public TimerEntityService(
            ITimerEntityRepository repository,
            ILogger<TimerEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<TimerEntityMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching TimerEntity for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return TimerEntityMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(TimerEntityMainDto dto)
        {
            _logger.LogInformation("Adding TimerEntity for Id {Id}", dto.Id);
            var entity = TimerEntityMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(TimerEntityMainDto dto)
        {
            _logger.LogInformation("Updating TimerEntity for Id {Id}", dto.Id);
            var entity = TimerEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task<IEnumerable<TimerEntityMainDto>> GetAll()
        {
            _logger.LogInformation("Fetching all TimerEntity data.");

            var entities = await _repository.GetAll();

            return entities.Select(x => TimerEntityMapper.ToMainDto(x));
        }

    }
}
    