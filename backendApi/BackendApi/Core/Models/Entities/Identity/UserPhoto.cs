using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserPhoto:BaseEntity
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}