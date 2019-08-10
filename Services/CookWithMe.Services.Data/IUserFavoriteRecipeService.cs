namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserFavoriteRecipeService
    {
        Task<bool> ContainsByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByUserIdAndRecipeIdAsync(string userId, string recipeId);

        Task<bool> DeleteByRecipeIdAsync(string recipeId);

        IQueryable<string> GetRecipeIdsByUserId(string userId);
    }
}
