using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithoutCanopy
{
    public class IFI_Bagfilter_Database_Without_CanopyService : IIFI_Bagfilter_Database_Without_CanopyService
    {
        private readonly IIFI_Bagfilter_Database_Without_CanopyRepository _repository;
        private readonly ILogger<IFI_Bagfilter_Database_Without_CanopyService> _logger;

        public IFI_Bagfilter_Database_Without_CanopyService(
            IIFI_Bagfilter_Database_Without_CanopyRepository repository,
            ILogger<IFI_Bagfilter_Database_Without_CanopyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IFI_Bagfilter_Database_Without_Canopy_Main_Dto> GetByProjectId(int id)
        {
            _logger.LogInformation("Fetching IFI_Bagfilter_Database_Without_Canopy for Id {Id}", id);
            var entity = await _repository.GetByProjectId(id);
            return IFI_Bagfilter_Database_Without_CanopyMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto)
        {
            _logger.LogInformation("Adding IFI_Bagfilter_Database_Without_Canopy for Id {Id}", dto.Id);
            var entity = IFI_Bagfilter_Database_Without_CanopyMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto)
        {
            _logger.LogInformation("Updating IFI_Bagfilter_Database_Without_Canopy for Id {Id}", dto.Id);
            var entity = IFI_Bagfilter_Database_Without_CanopyMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task<IFI_Bagfilter_Database_Without_Canopy_Main_Dto?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns)
        {
            _logger.LogInformation("Fetching IFI_Bagfilter by match criteria. processVolume: {ProcessVolume}, hopperType: {HopperType}, numberOfColumns: {NumberOfColumns}",
                processVolume, hopperType, numberOfColumns);

            var entity = await _repository.GetByMatchAsync(processVolume, hopperType, numberOfColumns);
            if (entity == null) return null;
            return IFI_Bagfilter_Database_Without_CanopyMapper.ToMainDto(entity);
        }

    }
}
    