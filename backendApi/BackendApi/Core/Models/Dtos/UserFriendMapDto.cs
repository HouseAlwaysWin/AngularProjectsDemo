using System;
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class UserFriendMapDto
    {
        public int FriendId { get; set; }
        public AppUserShortDto Friend { get; set; }
        public string MessageGroupId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
    }
}