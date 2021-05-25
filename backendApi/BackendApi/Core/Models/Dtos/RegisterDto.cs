using System;
namespace BackendApi.Core.Models.Dtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateTime  DateOfBirth { get; set; }
    }
}