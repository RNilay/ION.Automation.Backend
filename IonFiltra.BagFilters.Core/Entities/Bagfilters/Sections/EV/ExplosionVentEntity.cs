namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.EV
{
    public class ExplosionVentEntity
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
      
        public decimal? Explosion_Vent_Design_Pressure { get; set; }
        public int? Explosion_Vent_Quantity { get; set; }
        public string? Explosion_Vent_Size { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
