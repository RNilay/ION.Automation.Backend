namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.Sections.Access_Group
{
    public class AccessGroupMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public AccessGroupDto AccessGroup { get; set; }

    }
}
