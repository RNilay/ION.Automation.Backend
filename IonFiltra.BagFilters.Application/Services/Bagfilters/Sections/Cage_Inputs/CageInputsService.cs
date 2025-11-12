using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Cage_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Cage_Inputs
{
    public class CageInputsService : ICageInputsService
    {
        private readonly ICageInputsRepository _repository;
        private readonly ILogger<CageInputsService> _logger;

        public CageInputsService(
            ICageInputsRepository repository,
            ILogger<CageInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CageInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching CageInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return CageInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(CageInputsMainDto dto)
        {
            _logger.LogInformation("Adding CageInputs for Id {Id}", dto.Id);
            var entity = CageInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(CageInputsMainDto dto)
        {
            _logger.LogInformation("Updating CageInputs for Id {Id}", dto.Id);
            var entity = CageInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    