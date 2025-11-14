using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IBagfilterInputService
    {
        Task<BagfilterInputMainDto> GetByMasterId(int masterId);
        Task<List<BagfilterInputMainDto>> GetAllByEnquiryId(int enquiryId);
        Task<int> AddAsync(BagfilterInputMainDto dto);
        Task UpdateAsync(BagfilterInputMainDto dto);

        //Task<List<int>> AddRangeAsync(List<BagfilterInputMainDto> dtos);
        Task<AddRangeResultDto> AddRangeAsync(List<BagfilterInputMainDto> dtos, CancellationToken ct);
    }
}
    