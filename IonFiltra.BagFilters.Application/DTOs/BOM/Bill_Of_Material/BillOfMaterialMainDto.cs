namespace IonFiltra.BagFilters.Application.DTOs.BOM.Bill_Of_Material
{
    public class BillOfMaterialMainDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public int BagfilterMasterId { get; set; }
        public BillOfMaterialDto BillOfMaterial { get; set; }

    }
}
