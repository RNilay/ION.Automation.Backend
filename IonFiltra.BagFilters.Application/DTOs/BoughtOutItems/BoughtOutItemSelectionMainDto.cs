namespace IonFiltra.BagFilters.Application.DTOs.MasterData.BoughtOutItems
{
    public class BoughtOutItemSelectionMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public int? MasterDefinitionId { get; set; }
        public BoughtOutItemSelectionDto? BoughtOutItemSelection { get; set; }

        public SecondaryBoughtOutItemDto? SecondaryBoughtOutItem { get; set; }

    }
}
