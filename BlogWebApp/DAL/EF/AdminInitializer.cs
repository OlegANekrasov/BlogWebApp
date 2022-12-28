using BlogWebApp.DAL.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.DAL.EF
{
    public class AdminInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync("администратор"))
            {
                await roleManager.CreateAsync(new IdentityRole("администратор"));
            }

            if (!await roleManager.RoleExistsAsync("пользователь"))
            {
                await roleManager.CreateAsync(new IdentityRole("пользователь"));
            }

            if (!await roleManager.RoleExistsAsync("модератор"))
            {
                await roleManager.CreateAsync(new IdentityRole("модератор"));
            }

            var userManager = serviceProvider.GetService<UserManager<User>>();

            var user = await userManager.FindByNameAsync("olegnekrasov@live.com");
            if (user == null)
            {
                user = new User
                {
                    UserName = "olegnekrasov@live.com",
                    Email = "olegnekrasov@live.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "@1234567Nek");
                await userManager.AddToRoleAsync(user, "администратор");
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }
        }
    }
}
