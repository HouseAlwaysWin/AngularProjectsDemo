using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Models.Dtos
{
    public class Product_ProductAttributeDto
    {
        public int Id { get; set; }
        public int ProductAttributeId { get; set; }
        public ProductAttributeDto ProductAttribute { get; set; }
        public int ProductAttributeValueId { get; set; }
        public ProductAttributeValueDto ProductAttributeValue { get; set; } 
    }
}