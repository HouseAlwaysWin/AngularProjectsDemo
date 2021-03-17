namespace EcommerceApi.Core.Models.Entities
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(int productId,int quantity)
        {
            ProductInfoId = productId;
            Quantity = quantity; 
        }

        public int Id  { get; set; }
        public Product ProductInfo { get; set; }
        public int ProductInfoId { get; set; }
        public int Quantity { get; set; }
    }
}