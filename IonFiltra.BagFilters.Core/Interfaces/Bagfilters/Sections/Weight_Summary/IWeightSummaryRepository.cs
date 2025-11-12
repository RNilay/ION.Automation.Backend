using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Weight_Summary;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Weight_Summary
{
    public interface IWeightSummaryRepository
    {
        Task<WeightSummary?> GetById(int id);
        Task<int> AddAsync(WeightSummary entity);
        Task UpdateAsync(WeightSummary entity);
    }
}
    