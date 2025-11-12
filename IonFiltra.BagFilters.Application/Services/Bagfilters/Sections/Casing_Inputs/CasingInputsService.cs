using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Casing_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Casing_Inputs;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Casing_Inputs
{
    public class CasingInputsService : ICasingInputsService
    {
        private readonly ICasingInputsRepository _repository;
        private readonly ILogger<CasingInputsService> _logger;

        public CasingInputsService(
            ICasingInputsRepository repository,
            ILogger<CasingInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CasingInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching CasingInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return CasingInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(CasingInputsMainDto dto)
        {
            _logger.LogInformation("Adding CasingInputs for Id {Id}", dto.Id);
            var entity = CasingInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(CasingInputsMainDto dto)
        {
            _logger.LogInformation("Updating CasingInputs for Id {Id}", dto.Id);
            var entity = CasingInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    