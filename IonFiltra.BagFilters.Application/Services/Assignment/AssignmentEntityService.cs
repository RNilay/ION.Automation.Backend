using IonFiltra.BagFilters.Application.DTOs.Assignment;
using IonFiltra.BagFilters.Application.Interfaces;
using IonFiltra.BagFilters.Application.Mappers.Assignment;
using IonFiltra.BagFilters.Core.Entities.Assignment;
using IonFiltra.BagFilters.Core.Interfaces.Repositories.Assignment;
using Microsoft.Extensions.Logging;

namespace IonFiltra.BagFilters.Application.Services.Assignment
{
    public class AssignmentEntityService : IAssignmentEntityService
    {
        private readonly IAssignmentEntityRepository _repository;
        private readonly ILogger<AssignmentEntityService> _logger;

        public AssignmentEntityService(
            IAssignmentEntityRepository repository,
            ILogger<AssignmentEntityService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

       

        public async Task<List<AssignmentMainDto>> GetByUserId(int userId)
        {
            _logger.LogInformation("Fetching Assignments for UserID {UserId}", userId);

            var entity = await _repository.GetByUserId(userId);
            return entity.Select(AssignmentEntityMapper.ToMainDto).ToList();
        }

        public async Task<(List<AssignmentMainDto> Items, int TotalCount)> GetByUserId(int userId, int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching paginated Assignments for UserID {UserId}, Page {PageNumber}", userId, pageNumber);

            var (entities, totalCount) = await _repository.GetByUserId(userId, pageNumber, pageSize);

            var dtos = entities.Select(AssignmentEntityMapper.ToMainDto).ToList();

            return (dtos, totalCount);
        }

        public async Task<(List<AssignmentMainDto> Items, int TotalCount)> GetByEnquiryId(string enquiryId, int pageNumber, int pageSize)
        {
            _logger.LogInformation("Fetching paginated Assignments for EnquiryId {EnquiryId}, Page {PageNumber}", enquiryId, pageNumber);

            var (entities, totalCount) = await _repository.GetByEnquiryId(enquiryId, pageNumber, pageSize);

            var dtos = entities.Select(AssignmentEntityMapper.ToMainDto).ToList();

            return (dtos, totalCount);
        }



        //public async Task<List<AssignmentMainDto>> AddAsync(AssignmentRequest request)
        //{
        //    _logger.LogInformation("Adding {Count} AssignmentEntities for EnquiryId {EnquiryId}",
        //        request.ProcessVolumes, request.EnquiryId);

        //    var assignments = new List<AssignmentEntity>();
        //    var noOfAssignmentsToCreate = request.ProcessVolumes;
        //    for (int i = 1; i <= request.ProcessVolumes; i++)
        //    {
        //        assignments.Add(new AssignmentEntity
        //        {
        //            EnquiryId = request.EnquiryId,
        //            EnquiryAssignmentId = $"{request.EnquiryId}-{i}", // ✅ unique per row
        //            Customer = request.Customer,
        //            ProcessVolumes = request.ProcessVolumes,
        //            CreatedAt = DateTime.UtcNow
        //        });
        //    }

        //    var addedEntities =  await _repository.AddRangeAsync(assignments);
        //    return addedEntities.Select(AssignmentEntityMapper.ToMainDto).ToList();
        //}

        public async Task<List<AssignmentMainDto>> AddAsync(AssignmentRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.ProcessVolumes <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.ProcessVolumes), "ProcessVolumes must be greater than zero.");
            if (request.RequiredBagFilters < 0)
                throw new ArgumentOutOfRangeException(nameof(request.RequiredBagFilters), "RequiredBagFilters cannot be negative.");

            _logger.LogInformation("Adding {Count} AssignmentEntities for EnquiryId {EnquiryId}",
                request.ProcessVolumes, request.EnquiryId);

            var assignments = new List<AssignmentEntity>();

            int noOfAssignmentsToCreate = (int)Math.Round(
                (double)request.RequiredBagFilters / request.ProcessVolumes,
                MidpointRounding.AwayFromZero
            );

            // Toggle this according to desired behavior:
            bool minimumOneAssignment = true;

            // If there are zero requested bag filters and you don't want any assignments, keep zero:
            if (!minimumOneAssignment && request.RequiredBagFilters == 0)
            {
                return new List<AssignmentMainDto>();
            }

            if (minimumOneAssignment && noOfAssignmentsToCreate < 1)
            {
                noOfAssignmentsToCreate = 1;
            }

            for (int i = 1; i <= noOfAssignmentsToCreate; i++)
            {
                assignments.Add(new AssignmentEntity
                {
                    EnquiryId = request.EnquiryId,
                    EnquiryAssignmentId = $"{request.EnquiryId}-{i}",
                    Customer = request.Customer,
                    ProcessVolumes = request.ProcessVolumes,
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (assignments.Count == 0)
            {
                return new List<AssignmentMainDto>();
            }

            var addedEntities = await _repository.AddRangeAsync(assignments);
            return addedEntities.Select(AssignmentEntityMapper.ToMainDto).ToList();
        }






        public async Task UpdateAsync(AssignmentMainDto dto)
        {
            _logger.LogInformation("Updating AssignmentEntity for ProjectId {ProjectId}", dto.EnquiryId);
            var entity = AssignmentEntityMapper.ToEntity(dto);
            await _repository.UpdateAsync(entity);
        }
    }
}
    