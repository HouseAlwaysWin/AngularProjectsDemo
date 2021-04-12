using System.Collections.Generic;
using System.Reflection.Metadata;
namespace EcommerceApi.Core.Models.Entities
{
    public class ProductAttributeValue:BaseEntity
    {
       public int Id { get; set; } 
       public string Name { get; set; }
       public decimal PriceAdjustment { get; set; }
       public int Quantity { get; set; }
       public int SeqIndex { get; set; }
       public int ProductAttributeId { get; set; }
       public ProductAttribute ProductAttribute { get; set; }
    }
}