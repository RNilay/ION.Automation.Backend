namespace IonFiltra.BagFilters.Application.DTOs.BOM.Cage_Cost
{
    public class CageCostMainDto
    {
        public int Id { get; set; }
        public int? EnquiryId { get; set; }
        public int? BagfilterMasterId { get; set; }
        public CageCostEntityDto CageCostEntity { get; set; }

    }
}
