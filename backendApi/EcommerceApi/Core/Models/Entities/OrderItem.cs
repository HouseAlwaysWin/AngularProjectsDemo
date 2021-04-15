namespace EcommerceApi.Core.Models.Entities
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }

        // public long Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string ProductCategoryName { get; set; }
        public int Quantity { get; set; }
    }
}