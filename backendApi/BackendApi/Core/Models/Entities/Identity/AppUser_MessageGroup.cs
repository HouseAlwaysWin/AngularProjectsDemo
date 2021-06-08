using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class AppUser_MessageGroup
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int MessageGroupId { get; set; }
        public MessageGroup MessageGroup { get; set; }
    }
}