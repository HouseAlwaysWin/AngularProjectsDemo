using System.Collections.Generic;
using System.Globalization;

namespace EcommerceApi.Core.Models.Entities
{
    /// <summary>
    /// SPU
    /// </summary>
    public class Product:BaseEntity
    {
        public long Id  { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public ICollection<ProductSKU> ProductSKUs { get; set; }
    }
}