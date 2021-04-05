namespace EcommerceApi.Core.Models.Dtos
{
    public class OrderItemDto
    {
        
         public long Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string LangCode { get; set; }
        public int Quantity { get; set; }
        public string CreatedDate { get; set; } 
    }
}