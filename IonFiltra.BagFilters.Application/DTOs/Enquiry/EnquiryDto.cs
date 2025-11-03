namespace IonFiltra.BagFilters.Application.DTOs.Enquiry
{
    public class EnquiryDto
    {
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int RequiredBagFilters { get; set; }

        // Now a list (array) of integers
        public List<int> ProcessVolumes { get; set; } = new();

       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
