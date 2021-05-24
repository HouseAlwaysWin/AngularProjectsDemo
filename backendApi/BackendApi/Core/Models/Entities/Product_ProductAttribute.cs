using System;

namespace BackendApi.Core.Models.Entities
{
    public class Product_ProductAttribute:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}