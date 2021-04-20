namespace EcommerceApi.Core.Models.Entities
{
    public class BasketItem {
        public string Id  { get;  set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string ProductCategoryName { get; set; }
        public int Quantity { get; set; }
    }
}