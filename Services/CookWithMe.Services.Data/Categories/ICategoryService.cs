namespace CookWithMe.Services.Data.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models.Categories;

    public interface ICategoryService
    {
        Task<bool> CreateAllAsync(string[] titles);

        Task<IEnumerable<string>> GetAllTitlesAsync();

        Task<CategoryServiceModel> GetByIdAsync(int id);

        Task<bool> CreateAsync(CategoryServiceModel categoryServiceModel);

        Task<bool> EditAsync(CategoryServiceModel categoryServiceModel);

        Task<bool> DeleteByIdAsync(int id);

        Task SetCategoryToRecipeAsync(string categoryTitle, Recipe recipe);

        IQueryable<CategoryServiceModel> GetAll();

        Task<int> GetIdByTitleAsync(string categoryTitle);
    }
}
