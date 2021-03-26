namespace EcommerceApi.Core.Models.Entities
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(int productId,int quantity,Product productInfo)
        {
            ProductInfoId = productId;
            Quantity = quantity; 
            ProductInfo = productInfo;
        }

        public int Id  { get; set; }
        public Product ProductInfo { get; set; }
        public int ProductInfoId { get; set; }
        public int Quantity { get; set; }
    }
}