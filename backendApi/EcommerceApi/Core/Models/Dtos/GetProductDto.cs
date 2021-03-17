namespace EcommerceApi.Core.Models.Dtos
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public string LangCode { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
       
    }
}