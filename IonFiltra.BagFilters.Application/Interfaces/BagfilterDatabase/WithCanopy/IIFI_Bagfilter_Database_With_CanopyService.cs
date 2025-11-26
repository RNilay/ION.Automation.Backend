using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithCanopy;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IIFI_Bagfilter_Database_With_CanopyService
    {
        Task<IFI_Bagfilter_Database_With_Canopy_MainDto> GetById(int id);
        Task<int> AddAsync(IFI_Bagfilter_Database_With_Canopy_MainDto dto);
        Task UpdateAsync(IFI_Bagfilter_Database_With_Canopy_MainDto dto);

        Task<IFI_Bagfilter_Database_With_Canopy_MainDto?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns);
    }
}
    