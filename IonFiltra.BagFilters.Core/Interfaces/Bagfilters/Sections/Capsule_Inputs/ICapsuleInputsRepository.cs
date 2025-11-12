using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Capsule_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Capsule_Inputs
{
    public interface ICapsuleInputsRepository
    {
        Task<CapsuleInputs?> GetById(int id);
        Task<int> AddAsync(CapsuleInputs entity);
        Task UpdateAsync(CapsuleInputs entity);
    }
}
    