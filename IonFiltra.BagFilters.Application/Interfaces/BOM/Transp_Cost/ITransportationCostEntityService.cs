using IonFiltra.BagFilters.Application.DTOs.BOM.Transp_Cost;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface ITransportationCostEntityService
    {
        Task<TransportationCostMainDto> GetById(int id);
        Task<int> AddAsync(TransportationCostMainDto dto);
        Task UpdateAsync(TransportationCostMainDto dto);
    }
}
    