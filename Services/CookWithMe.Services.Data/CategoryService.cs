namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateAllAsync(string[] titles)
        {
            foreach (var title in titles)
            {
                var category = new Category
                {
                    Title = title,
                };

                await this.categoryRepository.AddAsync(category);
            }

            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateAsync(CategoryServiceModel model)
        {
            var category = AutoMapper.Mapper.Map<CategoryServiceModel, Category>(model);

            await this.categoryRepository.AddAsync(category);

            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<string> GetAllTitles()
        {
            return this.categoryRepository
                .AllAsNoTracking()
                .Select(x => x.Title);
        }

        public async Task SetCategoryToRecipe(string categoryTitle, Recipe recipe)
        {
            recipe.Category = await this.categoryRepository.All()
                .SingleOrDefaultAsync(x => x.Title == categoryTitle);
        }
    }
}
