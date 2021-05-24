using BackendApi.Core.Models.Entities;

namespace BackendApi.Core.Models.Dtos
{
    public class Product_PictureDto
    {
        public int Id { get; set; }
        public int PictureId { get; set; }
        public PictureDto Picture { get; set; }
        public int SeqNo { get; set; }
    }
}