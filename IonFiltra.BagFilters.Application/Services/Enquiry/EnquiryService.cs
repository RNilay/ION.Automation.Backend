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

        // ── Get paginated enquiries for ALL users EXCEPT the given userId ──────
        /// <summary>
        /// Returns enquiries that belong to every user other than <paramref name="userId"/>.
        /// Used for the "All Enquiries" dashboard tab so the current user can
        /// view and work on enquiries created by other users.
        /// </summary>
        public async Task<(List<EnquiryMainDto> Items, int TotalCount)> GetAllExceptUserId(
            int userId,
            int pageNumber,
            int pageSize)
        {
            _logger.LogInformation(
                "Fetching paginated Enquiries for all users excluding UserID {userId}",
                userId);

            var (entities, totalCount) =
                await _repository.GetAllExceptUser(userId, pageNumber, pageSize);

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

        // ── Update by EnquiryId + UserId ──────────────────────────────────────
        /// <summary>
        /// Updates editable fields (Customer, RequiredBagFilters) for the enquiry
        /// identified by its EnquiryId and owner UserId.
        /// The UserId in the DTO must be the OWNER's id, not necessarily the
        /// currently-logged-in user's id, so that cross-user edits work correctly.
        /// </summary>
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
                dto.Enquiry.RequiredBagFilters
            );
        }

        public async Task<bool> UpdateRequiredBagFiltersAsync(
        int enquiryId,
        int requiredBagFilters,
        CancellationToken ct)
        {
            if (enquiryId < 0)
                throw new ArgumentException("Invalid enquiryId");

            _logger.LogInformation(
                "Updating RequiredBagFilters for Enquiry {EnquiryId} to {Quota}",
                enquiryId,
                requiredBagFilters
            );

            return await _repository.UpdateRequiredBagFiltersAsync(
                enquiryId,
                requiredBagFilters,
                ct
            );
        }

    }
}
