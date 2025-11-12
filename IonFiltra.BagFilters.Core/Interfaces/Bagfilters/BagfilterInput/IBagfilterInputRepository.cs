using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterInputs;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.BagfilterMasterEntity;
using IonFiltra.BagFilters.Core.Entities.EnquiryEntity;

namespace IonFiltra.BagFilters.Core.Interfaces.Repositories.Bagfilters.BagfilterInputs
{
    public interface IBagfilterInputRepository
    {
        Task<BagfilterInput?> GetByProjectId(int projectId);
        Task<List<BagfilterInput>> GetByIdsAsync(IEnumerable<int> bagfilterInputIds);
        Task<int> AddAsync(BagfilterInput entity);
        Task UpdateAsync(BagfilterInput entity);

        Task<List<int>> AddRangeAsync(IEnumerable<(BagfilterMaster Master, BagfilterInput Input)> pairs);

        Task<List<int>> AddRangeAsync(
            IEnumerable<(BagfilterMaster Master, BagfilterInput Input)> pairs,
            Dictionary<int, (int matchedBagfilterInputId, int matchedBagfilterMasterId)>? matchMappingByPairIndex = null);

        Task<List<BagfilterInput>> FindCandidatesByGroupKeysAsync(IEnumerable<GroupKey> keys);
        Task<Dictionary<int, Enquiry>> GetEnquiriesByIdsAsync(IEnumerable<int> enquiryIds);

        Task UpdateS3dModelAsync(int bagfilterInputId, string s3dModelJson, string? sessionId);

    }
}
    