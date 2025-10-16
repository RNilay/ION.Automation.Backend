namespace IonFiltra.BagFilters.Application.DTOs.Enquiry
{
    public class EnquiryMainDto
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Creator
        public EnquiryDto Enquiry { get; set; }

    }
}
