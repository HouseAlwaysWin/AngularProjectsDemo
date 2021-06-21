namespace BackendApi.Core.Models.Dtos
{
    public class MessageGroupToAppUsersDto
    {
        public int AppUserId { get; set; }
        public AppGroupUserDto AppUser { get; set; }
    }
}