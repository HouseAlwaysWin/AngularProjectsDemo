using System;

namespace BackendApi.Core.Models.Entities
{
    public class OrderItem:IBaseEntity
    {
        public OrderItem()
        {
            
        }
       public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string ProductCategoryName { get; set; }
        public int Quantity { get; set; }
    }
}