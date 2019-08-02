namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Reviews;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IReviewService reviewService;

        public ReviewsController(IRecipeService recipeService, IUserService userService, IReviewService reviewService)
        { 
            this.recipeService = recipeService;
            this.userService = userService;
            this.reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            var recipeServiceModel = await this.recipeService.GetById(id);

            this.ViewData["RecipeId"] = id;
            this.ViewData["RecipeTitle"] = recipeServiceModel.Title;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["RecipeId"] = model.RecipeId;
                this.ViewData["RecipeTitle"] = model.RecipeTitle;

                return this.View();
            }

            var reviewServiceModel = model.To<ReviewServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reviewServiceModel.ReviewerId = userId;

            await this.reviewService.CreateAsync(reviewServiceModel);

            return this.Redirect($"/Recipes/Details/{reviewServiceModel.RecipeId}");
        }
    }
}
