namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Process_Info
{
    public class ProcessInfoMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public ProcessInfoDto ProcessInfo { get; set; }

    }
}
