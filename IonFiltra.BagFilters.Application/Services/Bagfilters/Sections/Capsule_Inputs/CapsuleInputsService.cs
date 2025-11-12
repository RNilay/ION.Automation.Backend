using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Capsule_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Capsule_Inputs
{
    public class CapsuleInputsService : ICapsuleInputsService
    {
        private readonly ICapsuleInputsRepository _repository;
        private readonly ILogger<CapsuleInputsService> _logger;

        public CapsuleInputsService(
            ICapsuleInputsRepository repository,
            ILogger<CapsuleInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CapsuleInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching CapsuleInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return CapsuleInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(CapsuleInputsMainDto dto)
        {
            _logger.LogInformation("Adding CapsuleInputs for Id {Id}", dto.Id);
            var entity = CapsuleInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(CapsuleInputsMainDto dto)
        {
            _logger.LogInformation("Updating CapsuleInputs for Id {Id}", dto.Id);
            var entity = CapsuleInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    