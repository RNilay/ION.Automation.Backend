using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Hopper_Trough;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Hopper_Trough
{
    public class HopperInputsService : IHopperInputsService
    {
        private readonly IHopperInputsRepository _repository;
        private readonly ILogger<HopperInputsService> _logger;

        public HopperInputsService(
            IHopperInputsRepository repository,
            ILogger<HopperInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<HopperInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching HopperInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return HopperInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(HopperInputsMainDto dto)
        {
            _logger.LogInformation("Adding HopperInputs for Id {Id}", dto.Id);
            var entity = HopperInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(HopperInputsMainDto dto)
        {
            _logger.LogInformation("Updating HopperInputs for Id {Id}", dto.Id);
            var entity = HopperInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    