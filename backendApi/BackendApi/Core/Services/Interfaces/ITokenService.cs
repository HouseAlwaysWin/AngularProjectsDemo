
using BackendApi.Core.Entities.Identity;

namespace BackendApi.Core.Services.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}