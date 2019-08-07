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

        Task<bool> CreateAsync(CategoryServiceModel serviceModel);

        Task<bool> EditAsync(CategoryServiceModel serviceModel);

        Task SetCategoryToRecipe(string categoryTitle, Recipe recipe);

        IQueryable<CategoryServiceModel> GetAll();
    }
}
