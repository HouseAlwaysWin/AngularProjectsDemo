namespace BackendApi.Core.Models.Dtos
{
    public class AppUserToMessageGroupsDto
    {
        public int MessageGroupId { get; set; }
        public MessageGroupDto MessageGroup { get; set; }
    }
}