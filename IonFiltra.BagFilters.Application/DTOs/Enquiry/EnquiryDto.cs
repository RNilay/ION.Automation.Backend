namespace IonFiltra.BagFilters.Application.DTOs.Enquiry
{
    public class EnquiryDto
    {
        public string EnquiryId { get; set; }
        public string Customer { get; set; }
        public int RequiredBagFilters { get; set; }

      

       
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
