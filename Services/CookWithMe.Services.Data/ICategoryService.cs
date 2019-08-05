namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface ICategoryService
    {
        Task<bool> CreateAllAsync(string[] titles);

        IQueryable<string> GetAllTitles();

        Task<CategoryServiceModel> GetById(int id);

        Task<bool> CreateAsync(CategoryServiceModel model);

        Task SetCategoryToRecipe(string categoryTitle, Recipe recipe);
    }
}
