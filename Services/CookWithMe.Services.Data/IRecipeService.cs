namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IRecipeService
    {
        Task<bool> CreateAsync(RecipeServiceModel model);

        Task<IEnumerable<RecipeServiceModel>> GetAllFiltered(string userId);

        Task<RecipeServiceModel> GetById(string id);

        Task SetRecipeToReview(string recipeId, Review review);

        Task<bool> SetRecipeToUserFavoriteRecipes(string userId, string recipeId);

        Task<bool> SetRecipeToUserCookedRecipes(string userId, string recipeId);
    }
}
