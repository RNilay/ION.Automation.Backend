using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems;

namespace IonFiltra.BagFilters.Application.Interfaces.BoughtOutItems
{
    public interface ISecondaryBoughtOutItemService
    {
        Task<List<SecondaryBoughtOutItem>> GetByEnquiryAsync(int enquiryId);
    }
}
