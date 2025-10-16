namespace IonFiltra.BagFilters.Core.Entities.EnquiryEntity
{
    public class Enquiry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int RequiredBagFilters { get; set; }

        // store as JSON in the DB (see DbContext config below)
        public List<int> ProcessVolumes { get; set; } = new();

        // keep Location as list (JSON) if you want; optional:
        public List<string> Location { get; set; } = new();

        public string? SubLocation { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}
