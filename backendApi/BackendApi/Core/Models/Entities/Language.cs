namespace BackendApi.Core.Models.Entities
{
    public class Language:BaseEntity
    {
    //    public int Id { get; set; }
       public string Name { get; set; }
       public string LangCulture { get; set; }
       public bool Published { get; set; }
       public int SeqNo { get; set; }
    }
}