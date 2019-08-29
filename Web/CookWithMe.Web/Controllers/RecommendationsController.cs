namespace CookWithMe.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data.Predictions;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Recommendations.Recommend;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RecommendationsController : BaseController
    {
        private readonly IPredictionService predictionService;
        private readonly IEnumParseService enumParseService;

        public RecommendationsController(
            IPredictionService predictionService,
            IEnumParseService enumParseService)
        {
            this.predictionService = predictionService;
            this.enumParseService = enumParseService;
        }

        [HttpGet]
        public async Task<IActionResult> Recommend()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recommendedRecipe = await this.predictionService.GetRecommendedRecipeAsync(userId);

            var recommendViewModel = recommendedRecipe
                .To<RecommendationRecommendViewModel>();
            recommendViewModel.NeededTimeDescription = this.enumParseService
                .GetEnumDescription(
                    Enum.GetName(typeof(Period), recommendedRecipe.NeededTime),
                    typeof(Period));

            return this.View(recommendViewModel);
        }
    }
}
