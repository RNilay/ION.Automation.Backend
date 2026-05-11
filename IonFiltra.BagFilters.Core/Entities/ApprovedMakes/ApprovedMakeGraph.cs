using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IonFiltra.BagFilters.Core.Entities.ApprovedMakes
{
    /// <summary>
    /// Carrier for the full approved-makes entity graph.
    /// Mirrors PaintSchemeGraph — used as the return type of the repository
    /// to avoid nullable-tuple inference issues with TransactionHelper.
    /// </summary>
    public class ApprovedMakeGraph
    {
        public EnquiryApprovedMake Header { get; set; } = null!;
        public List<EnquiryApprovedMakeItem> Items { get; set; } = new();
    }
}
