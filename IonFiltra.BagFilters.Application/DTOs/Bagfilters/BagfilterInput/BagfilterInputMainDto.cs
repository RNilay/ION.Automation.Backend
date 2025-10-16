using IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterMaster;

namespace IonFiltra.BagFilters.Application.DTOs.Bagfilters.BagfilterInputs
{
    public class BagfilterInputMainDto
    {
        public int BagfilterInputId { get; set; }
        public int BagfilterMasterId { get; set; }
        public BagfilterMasterDto? BagfilterMaster { get; set; }
        public BagfilterInputDto BagfilterInput { get; set; } = null!;

    }
}
