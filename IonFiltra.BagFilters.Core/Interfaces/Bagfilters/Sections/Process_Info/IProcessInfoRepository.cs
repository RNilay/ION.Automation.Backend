using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.Process_Info;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.Sections.Process_Info
{
    public interface IProcessInfoRepository
    {
        Task<ProcessInfo?> GetById(int id);
        Task<int> AddAsync(ProcessInfo entity);
        Task UpdateAsync(ProcessInfo entity);
    }
}
    