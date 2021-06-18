using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppleHardwareStore;
using AppleHardwareStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FurnitureFactory
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;

            FileHelper.Initialize(services.GetService<IWebHostEnvironment>());


            var rolesManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            //await RoleInitializer.InitializeRoleAsync(rolesManager);

            var userManager = services.GetRequiredService<UserManager<User>>();
            //await UserInitializer.InitializeUserAsync(userManager);

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}