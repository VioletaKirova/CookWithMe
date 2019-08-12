namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Reviews;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Reviews;
    using CookWithMe.Web.InputModels.Reviews.Create;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : BaseController
    {
        private const string CreateErrorMessage = "Failed to create the review.";
        private const string DeleteErrorMessage = "Failed to delete the review.";

        private readonly IReviewService reviewService;
        private readonly IRecipeService recipeService;

        public ReviewsController(
            IReviewService reviewService,
            IRecipeService recipeService)
        {
            this.reviewService = reviewService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);

            var reviewCreateInputModel = new ReviewCreateInputModel
            {
                RecipeId = id,
                RecipeTitle = recipeServiceModel.Title,
            };

            return this.View(reviewCreateInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateInputModel reviewCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var reviewServiceModel = reviewCreateInputModel.To<ReviewServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reviewServiceModel.ReviewerId = userId;

            if (!await this.reviewService.CreateAsync(reviewServiceModel))
            {
                this.TempData["Error"] = CreateErrorMessage;

                return this.View();
            }

            return this.Redirect($"/Recipes/Details/{reviewServiceModel.RecipeId}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await this.reviewService.DeleteByIdAsync(id))
            {
                this.TempData["Error"] = DeleteErrorMessage;
            }

            var recipeId = (await this.reviewService.GetByIdAsync(id)).RecipeId;

            return this.Redirect($"/Recipes/Details/{recipeId}");
        }
    }
}
