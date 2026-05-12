namespace IonFiltra.BagFilters.Core.Entities.EnquiryEntity
{
    public class Enquiry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int RequiredBagFilters { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // ── Soft-delete fields ─────────────────────────────────────────────────
        /// <summary>
        /// True once the record has been soft-deleted.
        /// All queries must filter WHERE IsDeleted = false.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Timestamp of when the soft-delete was performed. Null while active.
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }

}
