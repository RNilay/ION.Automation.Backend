using IonFiltra.BagFilters.Application.DTOs.Enquiry;
using IonFiltra.BagFilters.Application.Interfaces.Enquiry;
using IonFiltra.BagFilters.Application.Mappers.EnquiryMappper;
using IonFiltra.BagFilters.Core.Interfaces.EnquiryRep;
using Microsoft.Extensions.Logging;


namespace IonFiltra.BagFilters.Application.Services.EnquiryService
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IEnquiryRepository _repository;
        private readonly ILogger<EnquiryService> _logger;

        public EnquiryService(
            IEnquiryRepository repository,
            ILogger<EnquiryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<EnquiryMainDto>> GetByUserId(int userId)
        {
            _logger.LogInformation("Fetching Enquiries for UserID {userId}", userId);
            var entities = await _repository.GetByUserId(userId);

            return entities.Select(EnquiryMapper.ToMainDto).ToList();
        }

        public async Task<(List<EnquiryMainDto> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching paginated Enquiries for UserID {userId}", userId);

            var (entities, totalCount) = await _repository.GetByUserId(userId, pageNumber, pageSize);

            var dtos = entities.Select(EnquiryMapper.ToMainDto).ToList();

            return (dtos, totalCount);
        }


        public async Task<int> AddAsync(EnquiryMainDto dto)
        {
            _logger.LogInformation("Adding Enquiry for ProjectId {ProjectId}", dto.UserId);
            var entity = EnquiryMapper.ToEntity(dto);
            await _repository.AddAsync(entity);
            return entity.Id;
        }

        public async Task UpdateAsync(EnquiryMainDto dto)
        {
            _logger.LogInformation("Updating Enquiry for ProjectId {ProjectId}", dto.UserId);
            var entity = EnquiryMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateByEnquiryIdAsync(EnquiryMainDto dto)
        {
            if (dto == null || dto.Enquiry == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.LogInformation(
                "Updating Enquiry {EnquiryId} for User {UserId}",
                dto.Enquiry.EnquiryId,
                dto.UserId
            );

            return await _repository.UpdateByEnquiryIdAsync(
                dto.Enquiry.EnquiryId,
                dto.UserId,
                dto.Enquiry.Customer,
                dto.Enquiry.RequiredBagFilters,
                dto.Enquiry.ProcessVolumes
            );
        }

    }
}
