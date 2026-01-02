namespace IonFiltra.BagFilters.Core.Entities.BOM.Damper_Cost
    {
        public class DamperCostEntity
        {
            public int Id { get; set; }
        public int? EnquiryId { get; set; }
        public int? BagfilterMasterId {  get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Parameter { get; set; }
        public string? Value { get; set; }
        public string? Unit { get; set; }

        }
    }
    