﻿namespace CookWithMe.Web.Controllers
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
        private readonly IReviewService reviewService;
        private readonly IRecipeService recipeService;

        public ReviewsController(IReviewService reviewService, IRecipeService recipeService)
        {
            this.reviewService = reviewService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);

            var viewModel = new ReviewCreateInputModel
            {
                RecipeId = id,
                RecipeTitle = recipeServiceModel.Title,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReviewCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var reviewServiceModel = inputModel.To<ReviewServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            reviewServiceModel.ReviewerId = userId;

            await this.reviewService.CreateAsync(reviewServiceModel);

            return this.Redirect($"/Recipes/Details/{reviewServiceModel.RecipeId}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            await this.reviewService.DeleteByIdAsync(id);

            var recipeId = (await this.reviewService.GetByIdAsync(id)).RecipeId;

            return this.Redirect($"/Recipes/Details/{recipeId}");
        }
    }
}
