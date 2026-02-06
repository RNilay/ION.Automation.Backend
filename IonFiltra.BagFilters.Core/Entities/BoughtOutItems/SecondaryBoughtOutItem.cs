namespace IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems
{
    public class SecondaryBoughtOutItem
    {
        public int Id { get; set; }

        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }

        // Identifies the item (e.g. EXPANSION_JOINT, VISIT_ENGINEERING)
        public string MasterKey { get; set; } = null!;

        // Optional
        public string? Make { get; set; }
        public decimal? Cost { get; set; } //qty * rate

        // 🔥 NEW (BOM parity)
        public decimal? Qty { get; set; } = 1;
        public string? Unit { get; set; } = "No's";
        public decimal? Rate { get; set; }   // rate = per-unit

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
