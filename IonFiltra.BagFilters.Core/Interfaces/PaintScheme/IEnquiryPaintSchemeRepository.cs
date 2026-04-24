using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.PaintScheme;

namespace IonFiltra.BagFilters.Core.Interfaces.PaintScheme
{
    public interface IEnquiryPaintSchemeRepository
    {
        ///// <summary>
        ///// Saves the full paint scheme graph for an enquiry (header + sections
        ///// + BF assignments + overrides).  Call on first-time POST.
        ///// </summary>
        //Task<int> SaveAsync(
        //    EnquiryPaintScheme header,
        //    List<EnquiryPaintSchemeSection> sections,
        //    List<EnquiryPaintSchemeBfAssignment> assignments,
        //    List<(EnquiryPaintSchemeOverride Override, List<EnquiryPaintSchemeOverrideSection> Sections)> overrides
        //);

        ///// <summary>
        ///// Replaces the existing paint scheme graph for an enquiry.
        ///// Deletes old child rows and re-inserts fresh ones (upsert via delete+insert).
        ///// </summary>
        //Task<bool> UpdateAsync(
        //    int enquiryId,
        //    EnquiryPaintScheme header,
        //    List<EnquiryPaintSchemeSection> sections,
        //    List<EnquiryPaintSchemeBfAssignment> assignments,
        //    List<(EnquiryPaintSchemeOverride Override, List<EnquiryPaintSchemeOverrideSection> Sections)> overrides
        //);

        ///// <summary>
        ///// Returns the full paint scheme graph for the given enquiry,
        ///// or null if none has been saved yet.
        ///// </summary>
        //Task<(
        //    EnquiryPaintScheme? Header,
        //    List<EnquiryPaintSchemeSection> Sections,
        //    List<EnquiryPaintSchemeBfAssignment> Assignments,
        //    List<(EnquiryPaintSchemeOverride Override, List<EnquiryPaintSchemeOverrideSection> Sections)> Overrides
        //)?> GetByEnquiryIdAsync(int enquiryId);

        ///// <summary>
        ///// Checks whether a paint scheme record already exists for the given enquiry.
        ///// </summary>
        //Task<bool> ExistsByEnquiryIdAsync(int enquiryId);

        Task<int> SaveAsync(PaintSchemeGraph graph);

        Task<bool> UpdateAsync(int enquiryId, PaintSchemeGraph graph);

        /// <summary>Returns null if no paint scheme exists for the enquiry.</summary>
        Task<PaintSchemeGraph?> GetByEnquiryIdAsync(int enquiryId);

        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}
