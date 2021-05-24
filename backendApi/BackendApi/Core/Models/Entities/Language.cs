using System;

namespace BackendApi.Core.Models.Entities
{
    public class Language:IBaseEntity
    {
       public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
       public string Name { get; set; }
       public string LangCulture { get; set; }
       public bool Published { get; set; }
       public int SeqNo { get; set; }
    }
}