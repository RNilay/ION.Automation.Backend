namespace IonFiltra.BagFilters.Application.DTOs.Enquiry
{
    public class EnquiryDto
    {
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int RequiredBagFilters { get; set; }

        // Now a list (array) of integers
        public List<int> ProcessVolumes { get; set; } = new();

        // Optional — keep if you plan to use later; can be null
        public List<string> Location { get; set; } = new();

        public string? SubLocation { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
