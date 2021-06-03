namespace BackendApi.Core.Models.Dtos
{
    public class MessageParams: BasePaging
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}