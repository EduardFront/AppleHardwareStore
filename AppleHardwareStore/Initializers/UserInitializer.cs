using System.Threading.Tasks;
using AppleHardwareStore.Models;
using FurnitureFactory.Initializers;
using Microsoft.AspNetCore.Identity;
using static AppleHardwareStore.Startup;

namespace AppleHardwareStore.Initializers
{
    public static class UserInitializer
    {
        public static async Task InitializeUserAsync(UserManager<User> userManager)
        {
            var adminEmail = Configuration["AdminSetting:AdminEmail"];
            var password = Configuration["AdminSetting:AdminPassword"];
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Rolse.Admin);
                    admin.EmailConfirmed = true;
                }
            }
        }

    }
}
