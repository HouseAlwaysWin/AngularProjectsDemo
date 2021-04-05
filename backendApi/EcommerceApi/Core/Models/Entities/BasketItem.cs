namespace EcommerceApi.Core.Models.Entities
{
    public class BasketItem{
        public int Id  { get;  set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string LangCode { get; set; }
        public int ProductBrandId { get; set; }
        public int ProductCategoryId { get; set; }
        public int Quantity { get; set; }
    }
}