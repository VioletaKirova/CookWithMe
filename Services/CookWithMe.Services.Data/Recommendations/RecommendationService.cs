namespace CookWithMe.Services.Data.Recommendations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.MLModels.DataModels;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.ML;

    public class RecommendationService : IRecommendationService
    {
        public const double MinPredictionScore = 0.8;
        public const int MaxIterationsCount = 10;
        public const int DefaultIndex = 0;

        private readonly PredictionEnginePool<UserRecipe, UserRecipeScore> predictionEnginePool;
        private readonly IRecipeService recipeService;

        public RecommendationService(
            PredictionEnginePool<UserRecipe, UserRecipeScore> predictionEnginePool,
            IRecipeService recipeService)
        {
            this.predictionEnginePool = predictionEnginePool;
            this.recipeService = recipeService;
        }

        public async Task<RecipeServiceModel> GetRecommendedRecipeAsync(string userId)
        {
            var random = new Random();

            var filteredRecipes = await (await this.recipeService
                .GetAllFilteredAsync(userId))
                .OrderBy(x => random.Next())
                .Take(MaxIterationsCount)
                .ToListAsync();

            var userRecipe = new UserRecipe()
            {
                UserId = userId,
            };

            for (int i = 0; i < filteredRecipes.Count; i++)
            {
                userRecipe.RecipeId = filteredRecipes[i].Id;
                var prediction = this.predictionEnginePool.Predict<UserRecipe, UserRecipeScore>(userRecipe);

                if (prediction.Score >= MinPredictionScore ||
                    i == filteredRecipes.Count - 1)
                {
                    return filteredRecipes[i];
                }
            }

            return new RecipeServiceModel();
        }
    }
}
