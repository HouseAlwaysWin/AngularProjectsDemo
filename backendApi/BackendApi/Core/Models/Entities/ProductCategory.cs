using System;
using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities
{
    public class ProductCategory:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public bool HasChild { get; set; }
        public int SeqNo { get; set; }
        // public List<ProductCategory> Children { get; set; }
    }
}