namespace CookWithMe.Services.Data.Predictions
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Recipes;

    public interface IPredictionService
    {
        Task<RecipeServiceModel> GetRecommendedRecipeAsync(string userId);
    }
}
