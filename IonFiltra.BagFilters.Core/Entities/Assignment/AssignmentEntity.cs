namespace IonFiltra.BagFilters.Core.Entities.Assignment
{
    public class AssignmentEntity
    {
        public int Id { get; set; }
        public string EnquiryId { get; set; }
        public string EnquiryAssignmentId { get; set; }
        public string Customer { get; set; }
        public int ProcessVolumes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
