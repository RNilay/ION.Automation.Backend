using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.Bagfilters.Sections.PaintCostSummary;

namespace IonFiltra.BagFilters.Core.Interfaces.Bagfilters.Sections.PaintCostSummary
{
    public interface IBagfilterPaintingCostRepository
    {
        /// <summary>
        /// Inserts a new painting cost summary row for the BF.
        /// Called after a new BF is created (bagfilterMasterId is now known).
        /// </summary>
        Task SaveAsync(BagfilterPaintingCostSummary entity);

        /// <summary>
        /// Updates the existing painting cost summary for the BF.
        /// Called during batch update.
        /// </summary>
        Task<bool> UpdateAsync(BagfilterPaintingCostSummary entity);

        /// <summary>
        /// Upsert: inserts if no row exists for this BagfilterMasterId, updates otherwise.
        /// This is the recommended entry point — callers don't need to check existence.
        /// </summary>
        Task UpsertAsync(BagfilterPaintingCostSummary entity);

        /// <summary>
        /// Returns all painting cost summaries for an enquiry (for consolidated report).
        /// </summary>
        Task<List<BagfilterPaintingCostSummary>> GetByEnquiryIdAsync(int enquiryId);

        /// <summary>
        /// Returns the painting cost summary for a single BF.
        /// </summary>
        Task<BagfilterPaintingCostSummary?> GetByBagfilterMasterIdAsync(int bagfilterMasterId);

        Task UpsertRangeAsync(List<BagfilterPaintingCostSummary> entities);
    }
}
