using IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BOM.Bill_Of_Material;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BOM.Bill_Of_Material;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BOM.Bill_Of_Material
{
    public class BillOfMaterialService : IBillOfMaterialService
    {
        private readonly IBillOfMaterialRepository _repository;
        private readonly ILogger<BillOfMaterialService> _logger;

        public BillOfMaterialService(
            IBillOfMaterialRepository repository,
            ILogger<BillOfMaterialService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BillOfMaterialMainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching BillOfMaterial for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return BillOfMaterialMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(BillOfMaterialMainDto dto)
        {
            _logger.LogInformation("Adding BillOfMaterial for Id {Id}", dto.Id);
            var entity = BillOfMaterialMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(BillOfMaterialMainDto dto)
        {
            _logger.LogInformation("Updating BillOfMaterial for Id {Id}", dto.Id);
            var entity = BillOfMaterialMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    