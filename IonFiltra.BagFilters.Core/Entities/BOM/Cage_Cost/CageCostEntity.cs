namespace IonFiltra.BagFilters.Core.Entities.BOM.Cage_Cost
    {
        public class CageCostEntity
        {
            public int Id { get; set; }
        public int? EnquiryId { get; set; }
        public int? BagfilterMasterId { get; set; }
      
        public string? Parameter { get; set; }
        public string? Value { get; set; }
        public string? Unit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    }
    