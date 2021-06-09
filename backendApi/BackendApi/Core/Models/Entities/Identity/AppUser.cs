using System;
using System.Collections.Generic;
using BackendApi.Core.Models.Dtos;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendApi.Core.Entities.Identity
{
    public class AppUser : IdentityUser<int>,IBaseEntity
    {
        public string UserPublicId { get; set; }
        public string Alias { get; set; }
        public UserAddress Address { get; set; }
        public UserInfo UserInfo { get; set; }
        public DateTimeOffset CreatedDate { get; set; } 
        public DateTimeOffset? ModifiedDate { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }
        public virtual ICollection<AppUser_MessageGroup> MessageGroups { get; set; }
        public virtual ICollection<AppUserRole> AppUserRoles { get; set; }
        public virtual ICollection<UserPhoto> Photos { get; set; }
        public virtual ICollection<UserFriend> Friends { get; set; }
        public virtual ICollection<UserFriend> FriendsReverse { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}