using System;
using System.Collections.Generic;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserFriend 
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int FriendId { get; set; }
        public AppUser Friend { get; set; }
        public int MessageGroupId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}