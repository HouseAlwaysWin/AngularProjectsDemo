namespace EcommerceApi.Core.Models.Entities
{
    public class Product_Picture:BaseEntity
    {
        // public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
        public int SeqNo { get; set; }
    }
}