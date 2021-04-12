using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductCategoryDto
    {
       public int Id  { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public bool HasChild { get; set; }
        public int SeqNo { get; set; }
        public List<ProductCategoryDto> Children { get; set; }
    }
}