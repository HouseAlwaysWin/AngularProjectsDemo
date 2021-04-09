namespace EcommerceApi.Core.Models.Entities
{
    public class ProductTranslation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LangCode { get; set; }
    }
}