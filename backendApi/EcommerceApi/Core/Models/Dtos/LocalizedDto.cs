namespace EcommerceApi.Core.Models.Dtos
{
    public class LocalizedDto
    {
         public int Id { get; set; }
        public string LocaleTable { get; set; }
        public string LocaleKey { get; set; }
        public string LocaleValue { get; set; }
        public int LanguageId { get; set; }
        public LanguageDto Language { get; set; }
        public int TableId { get; set; } 
    }
}