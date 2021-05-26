using System.Collections.Generic;

namespace BackendApi.Core.Models.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public List<UserPhotoDto> Photos { get; set; }
    }
}