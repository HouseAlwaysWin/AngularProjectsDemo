using EcommerceApi.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Core.Models.Entities.Identity
{
    public class ECUserRole:IdentityUserRole<int>
    {
        public ECUser User { get; set; }
        public ECRole Role { get; set; }
    }
}