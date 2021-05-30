using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class UserFriendMapDto
    {
        public int FriendId { get; set; }
        public AppUserFriendDto Friend { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}