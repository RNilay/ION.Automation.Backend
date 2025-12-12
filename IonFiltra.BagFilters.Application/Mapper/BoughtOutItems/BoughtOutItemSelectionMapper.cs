using IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Application.Mappers.MasterData.BoughtOutItems
{
    public static class BoughtOutItemSelectionMapper
    {
        public static BoughtOutItemSelectionMainDto ToMainDto(BoughtOutItemSelection entity)
        {
            if (entity == null) return null;
            return new BoughtOutItemSelectionMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                BagfilterMasterId = entity.BagfilterMasterId,
                MasterDefinitionId = entity.MasterDefinitionId,
                BoughtOutItemSelection = new BoughtOutItemSelectionDto
                {
                    MasterKey = entity.MasterKey,
                    SelectedRowId = entity.SelectedRowId,
                },

            };
        }

        public static BoughtOutItemSelection ToEntity(BoughtOutItemSelectionMainDto dto)
        {
            if (dto == null) return null;
            return new BoughtOutItemSelection
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                BagfilterMasterId = dto.BagfilterMasterId,
                MasterDefinitionId = dto.MasterDefinitionId,
                MasterKey = dto.BoughtOutItemSelection.MasterKey,
                SelectedRowId = dto.BoughtOutItemSelection.SelectedRowId,

            };
        }
    }
}
