namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserCookedRecipeService
    {
        Task<bool> ContainsByUserIdAndRecipeId(string userId, string recipeId);

        Task<bool> DeleteByUserIdAndRecipeId(string userId, string recipeId);

        Task<bool> DeleteByRecipeId(string recipeId);
    }
}
