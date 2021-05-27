using System.Linq;
using System.Security.Claims;

namespace BackendApi.Helpers.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
       public static string GetUserName(this ClaimsPrincipal user){
           var name =  user.FindFirst(ClaimTypes.Name)?.Value;
           return name;
       } 

       public static string GetEmail(this ClaimsPrincipal user){
             var email = user.Claims?.FirstOrDefault(
                    x=>x.Type == ClaimTypes.Email)?.Value;
            return email;
       }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}