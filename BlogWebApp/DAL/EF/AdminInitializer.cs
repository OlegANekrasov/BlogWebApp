using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.DAL.EF
{
    public class AdminInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync("Администратор"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Администратор", "Роль с максимальными возможностями в приложении"));
            }

            if (!await roleManager.RoleExistsAsync("Пользователь"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Пользователь", "Стандартная роль приложения"));
            }

            if (!await roleManager.RoleExistsAsync("Модератор"))
            {
                await roleManager.CreateAsync(new ApplicationRole("Модератор", 
                    "Данная роль позволяет выполнять редактирование, удаление комментариев и статей в приложении"));
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
