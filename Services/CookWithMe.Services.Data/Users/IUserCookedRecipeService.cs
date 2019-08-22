namespace CookWithMe.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Recipes;

    public interface IUserCookedRecipeService
    {
        Task<bool> ContainsByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByRecipeIdAsync(string recipeId);

        IQueryable<RecipeServiceModel> GetRecipesByUserId(string userId);
    }
}
