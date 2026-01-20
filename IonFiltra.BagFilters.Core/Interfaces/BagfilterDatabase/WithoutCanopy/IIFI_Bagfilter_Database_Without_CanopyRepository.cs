using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithoutCanopy
{
    public interface IIFI_Bagfilter_Database_Without_CanopyRepository
    {
        Task<IFI_Bagfilter_Database_Without_Canopy?> GetByProjectId(int projectId);
        Task<int> AddAsync(IFI_Bagfilter_Database_Without_Canopy entity);
        Task UpdateAsync(IFI_Bagfilter_Database_Without_Canopy entity);
        Task<IFI_Bagfilter_Database_Without_Canopy?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns);

        Task<IFI_Bagfilter_Database_Without_Canopy?> GetByMatchAsync(
            string? processVolume,
            string? hopperType,
            decimal? numberOfColumns,
            decimal? baysX,
            decimal? baysZ
        );

    }
}
    