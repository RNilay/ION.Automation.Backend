namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.EV
{
    public class ExplosionVentEntityMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public ExplosionVentEntityDto ExplosionVentEntity { get; set; }

    }
}
