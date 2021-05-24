using System;
using System.Collections.Generic;
using System.Globalization;

namespace BackendApi.Core.Models.Entities
{
    public class Product:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public bool Deleted { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int ProductCategoryId { get; set; }
        public ICollection<Product_Picture> ProductPictureMap { get; set; }
        public ICollection<Product_ProductAttribute> ProductAttributeMap  { get; set; }

    }
}