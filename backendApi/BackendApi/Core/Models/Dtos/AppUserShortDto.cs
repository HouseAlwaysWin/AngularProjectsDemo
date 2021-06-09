using System;
using System.Collections.Generic;

namespace BackendApi.Core.Models.Dtos
{
    public class AppUserShortDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string UserPublicId { get; set; }
        public UserInfoDto UserInfo { get; set; }
        public string MainPhoto { get; set; } 
        public List<UserPhotoDto> Photos { get; set; }
    }
}