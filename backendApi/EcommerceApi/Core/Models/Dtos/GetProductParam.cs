namespace EcommerceApi.Core.Models.Dtos
{
    public class GetProductParam : BasePaging
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        public string Search { get; set; }
    }
}