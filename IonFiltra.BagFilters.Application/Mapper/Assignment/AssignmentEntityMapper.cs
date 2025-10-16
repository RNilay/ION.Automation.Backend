using IonFiltra.BagFilters.Application.DTOs.Assignment;
using IonFiltra.BagFilters.Core.Entities.Assignment;

namespace IonFiltra.BagFilters.Application.Mappers.Assignment
{
    public static class AssignmentEntityMapper
    {
        public static AssignmentMainDto ToMainDto(AssignmentEntity entity)
        {
            if (entity == null) return null;
            return new AssignmentMainDto
            {
                Id = entity.Id,
                EnquiryId = entity.EnquiryId,
                Assignment = new AssignmentDto
                {
                    EnquiryAssignmentId = entity.EnquiryAssignmentId,
                    Customer = entity.Customer,
                    ProcessVolumes = entity.ProcessVolumes,
                },

            };
        }

        public static AssignmentEntity ToEntity(AssignmentMainDto dto)
        {
            if (dto == null) return null;
            return new AssignmentEntity
            {
                Id = dto.Id,
                EnquiryId = dto.EnquiryId,
                EnquiryAssignmentId = dto.Assignment.EnquiryAssignmentId,
                Customer = dto.Assignment.Customer,
                ProcessVolumes = dto.Assignment.ProcessVolumes,
            };
        }
    }
}
