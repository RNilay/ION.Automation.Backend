using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;

namespace IonFiltra.BagFilters.Application.Interfaces.Bagfilters.BagfilterMaster
{
    public interface IBagfilterMasterService
    {
        Task<BagfilterMasterMainDto> GetByProjectId(int projectId);
        Task<int> AddAsync(BagfilterMasterMainDto dto);
        Task UpdateAsync(BagfilterMasterMainDto dto);
    }
}
