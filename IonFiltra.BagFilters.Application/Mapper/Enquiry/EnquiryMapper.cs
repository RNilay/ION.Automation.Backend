using IonFiltra.BagFilters.Application.DTOs.Enquiry;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;

namespace IonFiltra.BagFilters.Application.Mappers.EnquiryMappper
{
    public static class EnquiryMapper
    {
        public static EnquiryMainDto ToMainDto(Enquiry entity)
        {
            if (entity == null) return null;
            return new EnquiryMainDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Enquiry = new EnquiryDto
                {
                    EnquiryId = entity.EnquiryId,
                    Customer = entity.Customer,
                    RequiredBagFilters = entity.RequiredBagFilters,
                    ProcessVolumes = entity.ProcessVolumes,
                    Location = entity.Location,
                    SubLocation = entity.SubLocation,
                    CreatedAt = entity.CreatedAt
                },

            };
        }

        public static Enquiry ToEntity(EnquiryMainDto dto)
        {
            if (dto == null) return null;
            return new Enquiry
            {
                Id = dto.Id,
                UserId = dto.UserId,
                EnquiryId = dto.Enquiry.EnquiryId,
                Customer = dto.Enquiry.Customer,
                RequiredBagFilters = dto.Enquiry.RequiredBagFilters,
                ProcessVolumes = dto.Enquiry.ProcessVolumes ?? new List<int>(),
                Location = dto.Enquiry.Location ?? new List<string>(),
                SubLocation = dto.Enquiry.SubLocation,
                CreatedAt = dto.Enquiry.CreatedAt
            };
        }
    }

}
