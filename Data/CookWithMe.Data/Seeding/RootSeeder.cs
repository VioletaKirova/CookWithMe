namespace CookWithMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RootSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (userManager.Users.Any())
            {
                return;
            }

            var root = new ApplicationUser
            {
                UserName = "root_123",
                FullName = "Root Root",
                Email = "root@root.com",
            };

            var rootPassword = "root_123";

            var result = await userManager.CreateAsync(root, rootPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(root, GlobalConstants.AdministratorRoleName);
            }
        }
    }
}
