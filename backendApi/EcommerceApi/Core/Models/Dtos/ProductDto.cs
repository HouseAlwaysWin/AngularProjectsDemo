using System.Collections.Generic;
using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductDto
    {
        public int Id  { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public ProductCategoryDto ProductCategory { get; set; }
        public List<ProductAttributeDto> ProductAttributes { get; set; }
        // public List<ProductAttributeValueDto> ProductAttributeValues  { get; set; }
        public List<PictureDto> ProductPictures { get; set; }
       
    }
}