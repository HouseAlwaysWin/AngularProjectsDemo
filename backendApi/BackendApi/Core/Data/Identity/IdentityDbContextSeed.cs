using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendApi.Core.Entities.Identity;
using BackendApi.Core.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BackendApi.Core.Data.Identity
{
    public class IdentityDbContextSeed
    {
       public static async Task SeedUsersAsync(
           IdentityDbContext context,
           UserManager<AppUser> userManager,
           RoleManager<AppRole> roleManager,
           ILoggerFactory loggerFactory
           ){
            try{
                if (await userManager.Users.AnyAsync()) return;

                var userData = await System.IO.File.ReadAllTextAsync("./Core/Data/Identity/SeedData/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);
                if (users == null) return;

                var roles = new List<AppRole>
                {
                    new AppRole{Name = "Member"},
                    new AppRole{Name = "Admin"},
                    new AppRole{Name = "Moderator"},
                };

                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
                
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "111111");
                    await userManager.AddToRoleAsync(user, "Member");
                }

                var admin = new AppUser 
                {
                    Email = "admin@admin.com",
                    UserName = "admin",
                    Address = new UserAddress{
                        FirstName = "admin",
                        LastName = "admin",
                        Street = " 10 The Street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "234"
                    },
                    UserInfo = new UserInfo{
                        Gender= "female",
                        DateOfBirth= DateTime.Parse("1995-10-12"),
                        KnownAs= "Karen",
                        LastActive= DateTime.Parse( "2020-05-06"),
                        Introduction= "Laborum dolore aliquip voluptate sunt cupidatat fugiat. Aliqua cillum deserunt do sunt ullamco aute Lorem nisi irure velit esse excepteur ex qui. Aliquip cupidatat officia ullamco duis veniam quis elit dolore nisi proident exercitation id cillum ullamco. In exercitation aliqua commodo veniam culpa duis commodo mollit et sint culpa.\r\n"
                    },
                    Photos = new List<UserPhoto>(){
                        new UserPhoto{
                            Url="https://randomuser.me/api/portraits/women/50.jpg",
                            IsMain = true
                        }
                    }
                };

                await userManager.CreateAsync(admin,"123456");
                await userManager.AddToRolesAsync(admin, new[] {"Admin", "Moderator"}); 

            } catch(Exception ex){
                 var logger = loggerFactory.CreateLogger<StoreContext>();
                logger.LogError(ex.Message); 
            }
        }
    }
}