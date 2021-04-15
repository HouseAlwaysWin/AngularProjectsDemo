namespace EcommerceApi.Core.Models.Dtos
{
    public class ProductLikeParam:BasePaging
    {
        public string Search { get; set; }
        public bool LoadCategories { get; set; } 
        public bool LoadAttributes { get; set; }
        public bool LoadPictures { get; set; }
    }
}