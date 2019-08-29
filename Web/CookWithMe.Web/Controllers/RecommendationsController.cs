namespace CookWithMe.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data.Recommendations;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Recommendations.Recommend;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RecommendationsController : BaseController
    {
        private readonly IRecommendationService recommendationService;
        private readonly IEnumParseService enumParseService;

        public RecommendationsController(
            IRecommendationService recommendationService,
            IEnumParseService enumParseService)
        {
            this.recommendationService = recommendationService;
            this.enumParseService = enumParseService;
        }

        [HttpGet]
        public async Task<IActionResult> Recommend()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var recommendedRecipe = await this.recommendationService.GetRecommendedRecipeAsync(userId);

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
