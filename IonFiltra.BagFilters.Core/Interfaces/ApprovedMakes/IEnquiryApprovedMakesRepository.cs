using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.ApprovedMakes;

namespace IonFiltra.BagFilters.Core.Interfaces.ApprovedMakes
{
    public interface IEnquiryApprovedMakesRepository
    {
        /// <summary>
        /// Saves a new approved-makes record for the enquiry.
        /// Returns the new EnquiryApprovedMakes.Id.
        /// </summary>
        Task<int> SaveAsync(ApprovedMakeGraph graph);

        /// <summary>
        /// Replaces the existing approved-makes record for the enquiry
        /// using delete-children + re-insert (same pattern as PaintScheme).
        /// Returns false if no existing header was found.
        /// </summary>
        Task<bool> UpdateAsync(int enquiryId, ApprovedMakeGraph graph);

        /// <summary>
        /// Returns the full approved-makes graph for the given enquiry,
        /// or null if none has been saved yet.
        /// </summary>
        Task<ApprovedMakeGraph?> GetByEnquiryIdAsync(int enquiryId);

        /// <summary>
        /// Returns true if an approved-makes record already exists for the given enquiry.
        /// </summary>
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}
