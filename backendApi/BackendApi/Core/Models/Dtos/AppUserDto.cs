using System;
using System.Collections.Generic;
using BackendApi.Core.Models.Entities;
using BackendApi.Core.Models.Entities.Identity;

namespace BackendApi.Core.Models.Dtos
{
    public class AppUserDto
    {

        public string Token { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int PhoneNumber { get; set; }
        public bool PhoneNumberConfirm { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserPublicId { get; set; }
        public AddressDto Address { get; set; }
        public UserInfoDto UserInfo { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ModifiedDate { get; set; }
        public List<MessageDto> MessagesSent { get; set; }
        public ICollection<MessageDto> MessagesReceived { get; set; }
        public List<UserPhotoDto> Photos { get; set; }
    }
}