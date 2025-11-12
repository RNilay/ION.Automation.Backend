using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Structure_Inputs;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Structure_Inputs
{
    public class StructureInputsService : IStructureInputsService
    {
        private readonly IStructureInputsRepository _repository;
        private readonly ILogger<StructureInputsService> _logger;

        public StructureInputsService(
            IStructureInputsRepository repository,
            ILogger<StructureInputsService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<StructureInputsMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching StructureInputs for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return StructureInputsMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(StructureInputsMainDto dto)
        {
            _logger.LogInformation("Adding StructureInputs for Id {Id}", dto.Id);
            var entity = StructureInputsMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(StructureInputsMainDto dto)
        {
            _logger.LogInformation("Updating StructureInputs for Id {Id}", dto.Id);
            var entity = StructureInputsMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    