using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithCanopy;
using IonFiltra.BagFilters.Core.Entities.BagfilterDatabase.WithoutCanopy;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.BagfilterDatabase.WithCanopy
{
    public interface IIFI_Bagfilter_Database_With_CanopyRepository
    {
        Task<IFI_Bagfilter_Database_With_Canopy?> GetById(int id);
        Task<int> AddAsync(IFI_Bagfilter_Database_With_Canopy entity);
        Task UpdateAsync(IFI_Bagfilter_Database_With_Canopy entity);
        Task<IFI_Bagfilter_Database_With_Canopy?> GetByMatchAsync(string? processVolume, string? hopperType, decimal? numberOfColumns);

        Task<IFI_Bagfilter_Database_With_Canopy?> GetByMatchAsync(
            string? processVolume,
            string? hopperType,
            decimal? numberOfColumns,
            decimal? baysX,
            decimal? baysZ
        );


    }
}
    