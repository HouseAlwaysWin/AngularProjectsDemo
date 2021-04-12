using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductAttributeDto
    {
         public int Id { get; set; } 
       public string Name { get; set; }
       public List<ProductAttributeValueDto> ProductAttributeValue { get; set; }
    }
}