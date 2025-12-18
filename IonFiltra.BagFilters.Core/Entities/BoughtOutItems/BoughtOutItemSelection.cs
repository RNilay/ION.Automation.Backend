namespace IonFiltra.BagFilters.Core.Entities.MasterData.BoughtOutItems
{
    public class BoughtOutItemSelection
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public int? MasterDefinitionId { get; set; }
        public string? MasterKey { get; set; }
        public int? SelectedRowId { get; set; }

        //New
        public decimal? Qty { get; set; }
        public string? Unit { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Cost { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
