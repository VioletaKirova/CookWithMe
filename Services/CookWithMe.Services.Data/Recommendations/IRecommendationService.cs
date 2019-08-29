namespace CookWithMe.Services.Data.Recommendations
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Recipes;

    public interface IRecommendationService
    {
        Task<RecipeServiceModel> GetRecommendedRecipeAsync(string userId);
    }
}
