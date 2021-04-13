namespace EcommerceApi.Core.Models.Entities
{
    public class Localized:BaseEntity
    {
        // public int Id { get; set; }
        public string LocaleTable { get; set; }
        public string LocaleKey { get; set; }
        public string LocaleValue { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int TableId { get; set; }
    }
}