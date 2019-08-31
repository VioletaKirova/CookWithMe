namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Reviews;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Reviews;
    using CookWithMe.Web.InputModels.Reviews.Create;
    using CookWithMe.Web.InputModels.Reviews.Edit;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : BaseController
    {
        private const string CreateErrorMessage = "Failed to create the review.";
        private const string EditErrorMessage = "Failed to edit the review.";
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
                Recipe = new ReviewCreateRecipeInputModel
                {
                    Id = id,
                    Title = recipeServiceModel.Title,
                },
            };

            return this.View(reviewCreateInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateInputModel reviewCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(reviewCreateInputModel);
            }

            var reviewServiceModel = reviewCreateInputModel.To<ReviewServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reviewServiceModel.ReviewerId = userId;

            if (!await this.reviewService.CreateAsync(reviewServiceModel))
            {
                this.TempData["Error"] = CreateErrorMessage;

                return this.View(reviewCreateInputModel);
            }

            return this.Redirect($"/Recipes/Details/{reviewServiceModel.RecipeId}");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviewServiceModel = await this.reviewService.GetByIdAsync(id);

            if (userId != reviewServiceModel.ReviewerId)
            {
                return new ForbidResult();
            }

            var reviewEditInputModel = reviewServiceModel
                .To<ReviewEditInputModel>();

            reviewEditInputModel.Recipe = new ReviewEditRecipeInputModel
            {
                Id = reviewServiceModel.RecipeId,
                Title = (await this.recipeService.GetByIdAsync(reviewServiceModel.RecipeId)).Title,
            };

            return this.View(reviewEditInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, ReviewEditInputModel reviewEditInputModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var review = await this.reviewService.GetByIdAsync(id);

            if (userId != review.ReviewerId)
            {
                return new ForbidResult();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(reviewEditInputModel);
            }

            var reviewServiceModel = reviewEditInputModel.To<ReviewServiceModel>();

            reviewServiceModel.ReviewerId = userId;

            if (!await this.reviewService.EditAsync(id, reviewServiceModel))
            {
                this.TempData["Error"] = EditErrorMessage;

                return this.View(reviewEditInputModel);
            }

            return this.Redirect($"/Recipes/Details/{reviewServiceModel.RecipeId}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviewServiceModel = await this.reviewService.GetByIdAsync(id);

            if (userId != reviewServiceModel.ReviewerId &&
                !this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                return new ForbidResult();
            }

            if (!await this.reviewService.DeleteByIdAsync(id))
            {
                this.TempData["Error"] = DeleteErrorMessage;
            }

            var recipeId = (await this.reviewService.GetByIdAsync(id)).RecipeId;

            return this.Redirect($"/Recipes/Details/{recipeId}");
        }
    }
}
