using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.ApprovedMakes;

namespace IonFiltra.BagFilters.Application.Interfaces.ApprovedMakes
{
    public interface IApprovedMakesService
    {
        /// <summary>
        /// Saves approved makes for the enquiry.
        /// Returns the new EnquiryApprovedMakes.Id.
        /// </summary>
        Task<int> SaveAsync(SaveApprovedMakesRequestDto dto);

        /// <summary>
        /// Replaces existing approved makes for the enquiry.
        /// Returns false if no existing record was found.
        /// </summary>
        Task<bool> UpdateAsync(SaveApprovedMakesRequestDto dto);

        /// <summary>
        /// Returns the approved makes for the given enquiry, or null if none exist.
        /// </summary>
        Task<ApprovedMakesResponseDto?> GetByEnquiryIdAsync(int enquiryId);

        /// <summary>
        /// Returns true if approved makes already exist for the given enquiry.
        /// </summary>
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}
