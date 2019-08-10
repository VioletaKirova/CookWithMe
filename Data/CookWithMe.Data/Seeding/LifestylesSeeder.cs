namespace CookWithMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Lifestyles;

    using Microsoft.Extensions.DependencyInjection;

    internal class LifestylesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Lifestyles.Any())
            {
                return;
            }

            var lifestyleService = serviceProvider.GetRequiredService<ILifestyleService>();

            var lifestyleTypes = new string[]
            {
                "Omnivore",
                "Dairy Free",
                "Pescetarian",
                "Vegetarian",
                "Vegan",
            };

            await lifestyleService.CreateAllAsync(lifestyleTypes);
        }
    }
}
