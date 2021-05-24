using System;
using System.Collections.Generic;

namespace BackendApi.Core.Models.Entities
{
    public class Picture:IBaseEntity
    {
       public int Id { get ; set ; }
        public DateTimeOffset CreatedDate { get ; set ; }
        public DateTimeOffset? ModifiedDate { get ; set ; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public ICollection<Product_Picture>  ProductPictureMap { get; set; }
    }
}