namespace EcommerceApi.Core.Models.Entities
{
    public class Localized:BaseEntity
    {
        // public int Id { get; set; }
        public string EntityType { get; set; }
        public string PropertyKey { get; set; }
        public string PropertyValue { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int TableId { get; set; }
    }
}