using EcommerceApi.Core.Entities.Identity;

namespace EcommerceApi.Core.Services.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}