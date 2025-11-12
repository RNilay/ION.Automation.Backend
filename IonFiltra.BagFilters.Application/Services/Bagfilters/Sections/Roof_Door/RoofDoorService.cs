using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Roof_Door;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Roof_Door;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Roof_Door
{
    public class RoofDoorService : IRoofDoorService
    {
        private readonly IRoofDoorRepository _repository;
        private readonly ILogger<RoofDoorService> _logger;

        public RoofDoorService(
            IRoofDoorRepository repository,
            ILogger<RoofDoorService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RoofDoorMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching RoofDoor for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return RoofDoorMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(RoofDoorMainDto dto)
        {
            _logger.LogInformation("Adding RoofDoor for Id {Id}", dto.Id);
            var entity = RoofDoorMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(RoofDoorMainDto dto)
        {
            _logger.LogInformation("Updating RoofDoor for Id {Id}", dto.Id);
            var entity = RoofDoorMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    