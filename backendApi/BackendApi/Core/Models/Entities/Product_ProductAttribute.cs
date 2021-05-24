namespace BackendApi.Core.Models.Entities
{
    public class Product_ProductAttribute:BaseEntity
    {
        // public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductAttributeId { get; set; }
        public ProductAttribute ProductAttribute { get; set; }
    }
}