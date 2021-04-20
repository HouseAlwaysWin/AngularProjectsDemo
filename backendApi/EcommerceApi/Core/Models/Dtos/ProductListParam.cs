namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductListParam : BasePaging
    {
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
       
    }
}