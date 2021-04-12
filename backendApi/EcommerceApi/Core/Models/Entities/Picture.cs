using System.Collections.Generic;

namespace EcommerceApi.Core.Models.Entities
{
    public class Picture
    {
        public int Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public ICollection<Product_Picture>  ProductPictureMap { get; set; }
    }
}