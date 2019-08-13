namespace CookWithMe.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserFavoriteRecipeService
    {
        Task<bool> ContainsByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByRecipeIdAsync(string recipeId);

        Task<IEnumerable<string>> GetRecipeIdsByUserIdAsync(string userId);
    }
}
