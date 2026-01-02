using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.DamperSize;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.DamperSize;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.DamperSize
{
    public class DamperSizeInputsService : IDamperSizeInputsService
    {
        private readonly IDamperSizeInputsRepository _repository;
        private readonly ILogger<DamperSizeInputsService> _logger;

        public DamperSizeInputsService(
            IDamperSizeInputsRepository repository,
            ILogger<DamperSizeInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DamperSizeInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching DamperSizeInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return DamperSizeInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(DamperSizeInputsMainDto dto)
        {
            _logger.LogInformation("Adding DamperSizeInputs for Id {Id}", dto.Id);
            var entity = DamperSizeInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(DamperSizeInputsMainDto dto)
        {
            _logger.LogInformation("Updating DamperSizeInputs for Id {Id}", dto.Id);
            var entity = DamperSizeInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    