namespace CookWithMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Categories;

    using Microsoft.Extensions.DependencyInjection;

    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categoryService = serviceProvider.GetRequiredService<ICategoryService>();

            var categoryTitles = new string[]
            {
                "All",
                "Breakfast",
                "Snacks",
                "Salads",
                "Soups",
                "Main Dishes",
                "Desserts",
            };

            await categoryService.CreateAllAsync(categoryTitles);
        }
    }
}
