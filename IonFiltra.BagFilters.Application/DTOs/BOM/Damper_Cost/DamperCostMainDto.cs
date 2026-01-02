namespace IonFiltra.BagFilters.Application.DTOs.BOM.Damper_Cost
{
    public class DamperCostMainDto
    {
        public int Id { get; set; }
        public int? EnquiryId { get; set; }
        public int? BagfilterMasterId { get; set; }
        public DamperCostEntityDto DamperCostEntity { get; set; }

    }
}
