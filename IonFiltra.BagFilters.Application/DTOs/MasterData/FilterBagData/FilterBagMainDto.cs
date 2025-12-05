namespace IonFiltra.BagFilters.Application.DTOs.MasterData.FilterBagData
{
    public class FilterBagMainDto
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }  // NEW
        public FilterBagDto FilterBag { get; set; }

    }
}
