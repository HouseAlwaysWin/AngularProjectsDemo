using System;

namespace BackendApi.Core.Models.Entities
{
    public class Localized:IBaseEntity
    {
        public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string EntityType { get; set; }
        public string PropertyKey { get; set; }
        public string PropertyValue { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int TableId { get; set; }
    }
}