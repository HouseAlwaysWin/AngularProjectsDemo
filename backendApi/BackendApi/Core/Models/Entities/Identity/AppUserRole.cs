using BackendApi.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BackendApi.Core.Models.Entities.Identity
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}