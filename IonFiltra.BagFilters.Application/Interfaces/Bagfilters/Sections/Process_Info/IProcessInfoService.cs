using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IProcessInfoService
    {
        Task<ProcessInfoMainDto> GetById(int id);
        Task<int> AddAsync(ProcessInfoMainDto dto);
        Task UpdateAsync(ProcessInfoMainDto dto);
    }
}
    