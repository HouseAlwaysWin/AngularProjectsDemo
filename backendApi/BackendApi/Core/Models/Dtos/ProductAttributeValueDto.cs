namespace BackendApi.Core.Models.Dtos
{
    public class ProductAttributeValueDto
    {
       public int Id { get; set; } 
       public string Name { get; set; }
       public decimal PriceAdjustment { get; set; }
       public int SeqIndex { get; set; }
       public int ProductAttributeId { get; set; }
    }
}