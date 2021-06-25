namespace BackendApi.Core.Models.Dtos
{
    public class MessageParams: BasePaging
    {
        public int GroupId { get; set; }
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}