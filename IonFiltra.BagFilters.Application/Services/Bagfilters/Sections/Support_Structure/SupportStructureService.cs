using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Support_Structure;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Support_Structure;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Support_Structure
{
    public class SupportStructureService : ISupportStructureService
    {
        private readonly ISupportStructureRepository _repository;
        private readonly ILogger<SupportStructureService> _logger;

        public SupportStructureService(
            ISupportStructureRepository repository,
            ILogger<SupportStructureService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SupportStructureMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching SupportStructure for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return SupportStructureMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(SupportStructureMainDto dto)
        {
            _logger.LogInformation("Adding SupportStructure for Id {Id}", dto.Id);
            var entity = SupportStructureMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(SupportStructureMainDto dto)
        {
            _logger.LogInformation("Updating SupportStructure for Id {Id}", dto.Id);
            var entity = SupportStructureMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    