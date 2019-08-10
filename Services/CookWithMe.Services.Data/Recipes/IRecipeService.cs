namespace CookWithMe.Services.Data.Recipes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models.Recipes;

    public interface IRecipeService
    {
        Task<bool> CreateAsync(RecipeServiceModel recipeServiceModel);

        Task<bool> EditAsync(string id, RecipeServiceModel recipeServiceModel);

        Task<IQueryable<RecipeServiceModel>> GetAllFilteredAsync(string userId);

        Task<RecipeServiceModel> GetByIdAsync(string id);

        Task SetRecipeToReviewAsync(string recipeId, Review review);

        Task<bool> SetRecipeToUserFavoriteRecipesAsync(string userId, string recipeId);

        Task<bool> SetRecipeToUserCookedRecipesAsync(string userId, string recipeId);

        Task<bool> DeleteByIdAsync(string id);

        IQueryable<RecipeServiceModel> GetByCategoryId(int categoryId);

        IQueryable<RecipeServiceModel> GetByUserId(string userId);

        IQueryable<RecipeServiceModel> GetByIds(IEnumerable<string> recipeIds);

        Task<IQueryable<RecipeServiceModel>> GetBySearchValuesAsync(RecipeBrowseServiceModel recipeSearchServiceModel);
    }
}
