using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserPhoto: IBaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}