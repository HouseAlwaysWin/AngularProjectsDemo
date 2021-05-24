using System.Collections.Generic;
using BackendApi.Helpers;

namespace BackendApi.Core.Models.Dtos
{
    public class ProductCategoryDto:ITreeNode<ProductCategoryDto>
    {
       public int Id  { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public bool HasChild { get; set; }
        public int SeqNo { get; set; }
        public IEnumerable<ProductCategoryDto> Children { get; set; }
    }
}