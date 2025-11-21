using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Bagfilters.Sections.Painting;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Painting;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.Sections.Painting
{
    public class PaintingAreaService : IPaintingAreaService
    {
        private readonly IPaintingAreaRepository _repository;
        private readonly ILogger<PaintingAreaService> _logger;

        public PaintingAreaService(
            IPaintingAreaRepository repository,
            ILogger<PaintingAreaService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PaintingAreaMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching PaintingArea for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return PaintingAreaMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(PaintingAreaMainDto dto)
        {
            _logger.LogInformation("Adding PaintingArea for Id {Id}", dto.Id);
            var entity = PaintingAreaMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(PaintingAreaMainDto dto)
        {
            _logger.LogInformation("Updating PaintingArea for Id {Id}", dto.Id);
            var entity = PaintingAreaMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    