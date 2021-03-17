namespace EcommerceApi.Core.Models.Dtos
{
    public class GetProductParam : BasePaging
    {
        public string LangCode { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        public string Search { get; set; }
    }
}