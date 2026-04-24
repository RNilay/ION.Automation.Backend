using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Application.DTOs.PaintScheme;

namespace IonFiltra.BagFilters.Application.Interfaces.PaintScheme
{
    public interface IPaintSchemeService
    {
        /// <summary>
        /// Saves a new paint scheme for the enquiry.
        /// Returns the new EnquiryPaintScheme.Id.
        /// </summary>
        Task<int> SaveAsync(SavePaintSchemeRequestDto dto);

        /// <summary>
        /// Replaces the existing paint scheme for the enquiry.
        /// Returns false if no existing record was found.
        /// </summary>
        Task<bool> UpdateAsync(SavePaintSchemeRequestDto dto);

        /// <summary>
        /// Returns the full paint scheme for the given enquiry,
        /// or null if none exists.
        /// </summary>
        Task<PaintSchemeResponseDto?> GetByEnquiryIdAsync(int enquiryId);

        /// <summary>
        /// Returns true if a paint scheme already exists for the given enquiry.
        /// Used by the controller to decide whether to call Save or Update.
        /// </summary>
        Task<bool> ExistsByEnquiryIdAsync(int enquiryId);
    }
}
