using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster;
using IonFiltra.BagFilters.Application.Mapper.Bagfilters.BagfilterMasters;
using IonFiltra.BagFilters.Core.Interfaces.Bagfilters.BagfilterMasters;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Bagfilters.BagfilterMasterEntity
{
    public class BagfilterMasterService : IBagfilterMasterService
    {
        private readonly IBagfilterMasterRepository _repository;
        private readonly ILogger<BagfilterMasterService> _logger;

        public BagfilterMasterService(
            IBagfilterMasterRepository repository,
            ILogger<BagfilterMasterService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<BagfilterMasterMainDto> GetByProjectId(int projectId)
        {
            _logger.LogInformation("Fetching BagfilterMaster for ProjectId {ProjectId}", projectId);
            var entity = await _repository.GetByProjectId(projectId);
            return BagfilterMasterMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(BagfilterMasterMainDto dto)
        {
            _logger.LogInformation("Adding BagfilterMaster for AssignmentId {AssignmentId}", dto.BagfilterMaster.AssignmentId);
            var entity = BagfilterMasterMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.BagfilterMasterId;
        }

        public async Task UpdateAsync(BagfilterMasterMainDto dto)
        {
            _logger.LogInformation("Updating BagfilterMaster for AssignmentId {AssignmentId}", dto.BagfilterMaster.AssignmentId);
            var entity = BagfilterMasterMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
