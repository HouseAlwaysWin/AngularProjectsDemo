using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Entities
{
    public class ProductAttribute:BaseEntity
    {
       public int Id { get; set; } 
       public string Name { get; set; }
       public ICollection<ProductAttributeValue> ProductAttributeValue { get; set; }
       public ICollection<Product_ProductAttribute> ProductAttributeMap  { get; set; }
    }
}