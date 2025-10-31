using IonFiltra.BagFilters.Application.DTOs.BagfilterDatabase.WithoutCanopy;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IIFI_Bagfilter_Database_Without_CanopyService
    {
        Task<IFI_Bagfilter_Database_Without_Canopy_Main_Dto> GetByProjectId(int projectId);
        Task<int> AddAsync(IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto);
        Task UpdateAsync(IFI_Bagfilter_Database_Without_Canopy_Main_Dto dto);
        Task<IFI_Bagfilter_Database_Without_Canopy_Main_Dto?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns);

    }
}
    