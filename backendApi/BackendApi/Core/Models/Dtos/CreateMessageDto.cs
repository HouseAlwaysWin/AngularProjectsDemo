namespace BackendApi.Core.Models.Dtos
{
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; } 
    }
}