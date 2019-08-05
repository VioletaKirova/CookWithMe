namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateAllAsync(string[] categoryTitles)
        {
            foreach (var categoryTitle in categoryTitles)
            {
                var category = new Category
                {
                    Title = categoryTitle,
                };

                await this.categoryRepository.AddAsync(category);
            }

            var result = await this.categoryRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel)
        {
            var category = categoryServiceModel.To<Category>();

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

        public async Task<CategoryServiceModel> GetById(int id)
        {
            var category = await this.categoryRepository
                .GetByIdWithDeletedAsync(id);

            return category.To<CategoryServiceModel>();
        }

        public async Task SetCategoryToRecipe(string categoryTitle, Recipe recipe)
        {
            recipe.Category = await this.categoryRepository.All()
                .SingleOrDefaultAsync(x => x.Title == categoryTitle);
        }
    }
}
