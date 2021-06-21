using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class MessageRecivedUser
    {
        public int Id { get; set; }
        public Message Message { get; set; }
        public int MessageGroupId { get; set; }
        public int AppUserId { get; set; }
        // public AppUser AppUser { get; set; }
        public string UserName { get; set; }
        public string UserMainPhoto { get; set; }
        public DateTimeOffset? DateRead { get; set; }
    }
}