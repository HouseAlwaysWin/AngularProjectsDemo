using System;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class UserInfo: IBaseEntity
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTimeOffset LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }

        public int GetAge(){
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if(DateOfBirth > today.AddYears(-age)){
                age--;
            }
            return age;
        }
    }
}