using System.Linq;
using System.Threading.Tasks;
using EcommerceApi.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApi.Core.Data.Identity
{
    public class ECIdentityDbContextSeed
    {
       public static async Task SeedUsersAsync(UserManager<ECUser> userManager){
            if(!userManager.Users.Any()){
                var user = new ECUser
                {
                    DisplayName="admin",
                    Email = "admin@admin.com",
                    UserName = "admi",
                    Address = new UserAddress{
                        FirstName = "admin",
                        LastName = "admin",
                        Street = " 10 The Street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "234"
                    }
                };

                await userManager.CreateAsync(user,"123456");
            }
        } 
    }
}