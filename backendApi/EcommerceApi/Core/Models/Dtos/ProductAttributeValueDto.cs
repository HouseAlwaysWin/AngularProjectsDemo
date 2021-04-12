namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductAttributeValueDto
    {
       public int Id { get; set; } 
       public string Name { get; set; }
       public decimal PriceAdjustment { get; set; }
       public int Quantity { get; set; }
       public int SeqIndex { get; set; }
    }
}