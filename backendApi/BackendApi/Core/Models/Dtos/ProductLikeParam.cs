namespace BackendApi.Core.Models.Dtos
{
    public class ProductLikeParam:BasePaging
    {
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        public string Search { get; set; }
        public bool LoadCategories { get; set; } 
        public bool LoadAttributes { get; set; }
        public bool LoadPictures { get; set; }
    }
}