namespace CookWithMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;

    using Microsoft.Extensions.DependencyInjection;

    internal class AllergensSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Allergens.Any())
            {
                return;
            }

            var allergenService = serviceProvider.GetRequiredService<IAllergenService>();

            var allergenNames = new string[]
            {
                "Milk",
                "Eggs",
                "Fish",
                "Crustaceans",
                "Tree nuts",
                "Peanuts",
                "Wheat",
                "Soybeans",
            };

            await allergenService.CreateAsync(allergenNames);
        }
    }
}
