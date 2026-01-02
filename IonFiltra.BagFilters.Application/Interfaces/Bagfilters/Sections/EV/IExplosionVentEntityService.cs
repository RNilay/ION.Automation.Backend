using IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.EV;

namespace IonFiltra.BagFilters.Application.Interfaces
{
    public interface IExplosionVentEntityService
    {
        Task<ExplosionVentEntityMainDto> GetById(int id);
        Task<int> AddAsync(ExplosionVentEntityMainDto dto);
        Task UpdateAsync(ExplosionVentEntityMainDto dto);
    }
}
    