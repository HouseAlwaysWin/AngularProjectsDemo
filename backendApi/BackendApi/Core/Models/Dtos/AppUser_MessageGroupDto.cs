namespace BackendApi.Core.Models.Dtos
{
    public class AppUser_MessageGroupDto
    {
        public int AppUserId { get; set; }
        public AppUserDto AppUser { get; set; }
        public int MessageGroupId { get; set; }
        public MessageGroupDto MessageGroup { get; set; }
    }
}