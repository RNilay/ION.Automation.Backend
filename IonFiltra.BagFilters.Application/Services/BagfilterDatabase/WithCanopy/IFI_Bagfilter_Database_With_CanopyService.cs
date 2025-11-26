using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Application.Mappers.BagfilterDatabase.WithoutCanopy;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithCanopy;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.BagfilterDatabase.WithCanopy
{
    public class IFI_Bagfilter_Database_With_CanopyService : IIFI_Bagfilter_Database_With_CanopyService
    {
        private readonly IIFI_Bagfilter_Database_With_CanopyRepository _repository;
        private readonly ILogger<IFI_Bagfilter_Database_With_CanopyService> _logger;

        public IFI_Bagfilter_Database_With_CanopyService(
            IIFI_Bagfilter_Database_With_CanopyRepository repository,
            ILogger<IFI_Bagfilter_Database_With_CanopyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IFI_Bagfilter_Database_With_Canopy_MainDto> GetById(int id)
        {
            _logger.LogInformation("Fetching IFI_Bagfilter_Database_With_Canopy for Id {Id}", id);
            var entity = await _repository.GetById(id);
            return IFI_Bagfilter_Database_With_CanopyMapper.ToMainDto(entity);
        }

        public async Task<int> AddAsync(IFI_Bagfilter_Database_With_Canopy_MainDto dto)
        {
            _logger.LogInformation("Adding IFI_Bagfilter_Database_With_Canopy for Id {Id}", dto.Id);
            var entity = IFI_Bagfilter_Database_With_CanopyMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(IFI_Bagfilter_Database_With_Canopy_MainDto dto)
        {
            _logger.LogInformation("Updating IFI_Bagfilter_Database_With_Canopy for Id {Id}", dto.Id);
            var entity = IFI_Bagfilter_Database_With_CanopyMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task<IFI_Bagfilter_Database_With_Canopy_MainDto?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns)
        {
            _logger.LogInformation("Fetching IFI_Bagfilter by match criteria. processVolume: {ProcessVolume}, hopperType: {HopperType}, numberOfColumns: {NumberOfColumns}",
                processVolume, hopperType, numberOfColumns);

            var entity = await _repository.GetByMatchAsync(processVolume, hopperType, numberOfColumns);
            if (entity == null) return null;
            return IFI_Bagfilter_Database_With_CanopyMapper.ToMainDto(entity);
        }
    }
}
    