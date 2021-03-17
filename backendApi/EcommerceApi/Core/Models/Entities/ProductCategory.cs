using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Entities
{
    public class ProductCategory:BaseEntity
    {
        public int Id  { get; set; }
        public string Name { get; set; }
    }
}