using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Structure_Inputs;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Structure_Inputs
{
    public interface IStructureInputsRepository
    {
        Task<StructureInputs?> GetById(int id);
        Task<int> AddAsync(StructureInputs entity);
        Task UpdateAsync(StructureInputs entity);
    }
}
    