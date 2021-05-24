using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities
{
    public class ProductAttribute:BaseEntity
    {
       public string Name { get; set; }
       public ICollection<ProductAttributeValue> ProductAttributeValue { get; set; }
       public ICollection<Product_ProductAttribute> ProductAttributeMap  { get; set; }
    }
}