namespace BackendApi.Core.Models.Dtos
{
    public class PictureDto
    {
        public int Id { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
    }
}