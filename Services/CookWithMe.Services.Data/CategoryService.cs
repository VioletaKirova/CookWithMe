namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoryService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateAsync(string[] titles)
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
    }
}
