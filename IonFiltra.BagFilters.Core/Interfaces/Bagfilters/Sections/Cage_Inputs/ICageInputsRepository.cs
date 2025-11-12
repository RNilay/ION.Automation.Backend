using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Cage_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Cage_Inputs
{
    public interface ICageInputsRepository
    {
        Task<CageInputs?> GetById(int id);
        Task<int> AddAsync(CageInputs entity);
        Task UpdateAsync(CageInputs entity);
    }
}
    