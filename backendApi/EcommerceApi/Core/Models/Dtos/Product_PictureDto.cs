using EcommerceApi.Core.Models.Entities;

namespace EcommerceApi.Core.Models.Dtos
{
    public class Product_PictureDto
    {
        public int Id { get; set; }
        public int PictureId { get; set; }
        public PictureDto Picture { get; set; }
        public int SeqNo { get; set; }
    }
}