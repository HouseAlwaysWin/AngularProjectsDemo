using System;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserInfo:BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }

        public int GetAge(){
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if(DateOfBirth.Date >today.AddYears(-age)){
                age--;
            }
            return age;
        }
    }
}