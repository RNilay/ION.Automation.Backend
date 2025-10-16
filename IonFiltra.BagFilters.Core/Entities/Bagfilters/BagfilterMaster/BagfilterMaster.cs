using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;

namespace IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity
{
    public class BagfilterMaster
    {
        public int BagfilterMasterId { get; set; }
        public int? AssignmentId { get; set; }
        public int? EnquiryId { get; set; }
        public string BagFilterName { get; set; }
        public string? Status { get; set; }
        public int? Revision { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // <-- add this navigation collection
        public ICollection<BagfilterInput>? Inputs { get; set; } = new List<BagfilterInput>();

    }
}
