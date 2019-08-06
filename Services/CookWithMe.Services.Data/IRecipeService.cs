namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IRecipeService
    {
        Task<bool> CreateAsync(RecipeServiceModel model);

        Task<bool> Edit(string id, RecipeServiceModel model);

        Task<IQueryable<RecipeServiceModel>> GetAllFiltered(string userId);

        Task<RecipeServiceModel> GetById(string id);

        Task SetRecipeToReview(string recipeId, Review review);

        Task<bool> SetRecipeToUserFavoriteRecipes(string userId, string recipeId);

        Task<bool> SetRecipeToUserCookedRecipes(string userId, string recipeId);

        Task<bool> Delete(string id);

        IQueryable<RecipeServiceModel> GetAllByCategoryId(int categoryId);

        IQueryable<RecipeServiceModel> GetByIds(IEnumerable<string> recipeIds);
    }
}
