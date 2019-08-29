namespace CookWithMe.Services.Data.Predictions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.MLModels.DataModels;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.ML;

    public class PredictionService : IPredictionService
    {
        private readonly PredictionEnginePool<UserRecipe, UserRecipeScore> predictionEnginePool;
        private readonly IRecipeService recipeService;

        public PredictionService(
            PredictionEnginePool<UserRecipe, UserRecipeScore> predictionEnginePool,
            IRecipeService recipeService)
        {
            this.predictionEnginePool = predictionEnginePool;
            this.recipeService = recipeService;
        }

        public async Task<RecipeServiceModel> GetRecommendedRecipeAsync(string userId)
        {
            var filteredRecipeIds = await (await this.recipeService
                .GetAllFilteredAsync(userId))
                .Select(x => x.Id)
                .ToListAsync();

            var userRecipe = new UserRecipe()
            {
                UserId = userId,
            };

            var random = new Random();
            var max = filteredRecipeIds.Count;
            var count = 0;

            while (true)
            {
                count++;
                var index = random.Next(0, max);
                var recipeId = filteredRecipeIds[index];

                userRecipe.RecipeId = recipeId;

                var prediction = this.predictionEnginePool.Predict<UserRecipe, UserRecipeScore>(userRecipe);

                if (prediction.Score > GlobalConstants.MinimumPredictionScore ||
                    count == GlobalConstants.MaxIterationsCount)
                {
                    return await this.recipeService.GetByIdAsync(recipeId);
                }
            }
        }
    }
}
