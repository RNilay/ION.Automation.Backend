using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Hopper_Trough;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Hopper_Trough
{
    public interface IHopperInputsRepository
    {
        Task<HopperInputs?> GetById(int id);
        Task<int> AddAsync(HopperInputs entity);
        Task UpdateAsync(HopperInputs entity);
    }
}
    