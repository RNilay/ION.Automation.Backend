using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IWeightSummaryService
    {
        Task<WeightSummaryMainDto> GetById(int id);
        Task<int> AddAsync(WeightSummaryMainDto dto);
        Task UpdateAsync(WeightSummaryMainDto dto);
    }
}
    