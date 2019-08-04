namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Recipes;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IUserFavoriteRecipeService userFavoriteRecipeService;
        private readonly IUserCookedRecipeService userCookedRecipeService;
        private readonly IReviewService reviewService;
        private readonly IStringFormatService stringFormatService;

        public RecipesController(
            IRecipeService recipeService,
            IUserService userService,
            IUserFavoriteRecipeService userFavoriteRecipeService,
            IUserCookedRecipeService userCookedRecipeService,
            IReviewService reviewService,
            IStringFormatService stringFormatService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.userFavoriteRecipeService = userFavoriteRecipeService;
            this.userCookedRecipeService = userCookedRecipeService;
            this.reviewService = reviewService;
            this.stringFormatService = stringFormatService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var recipeServiceModel = await this.recipeService.GetById(id);
            recipeServiceModel.Reviews = await this.reviewService.GetAllByRecipeId(id);

            var recipeViewModel = recipeServiceModel.To<RecipeDetailsViewModel>();

            recipeViewModel.DirectionsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.Directions);

            recipeViewModel.ShoppingListIngredients = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.ShoppingList.Ingredients);

            recipeViewModel.FormatedPreparationTime = this.stringFormatService
                .DisplayTime(recipeServiceModel.PreparationTime);

            recipeViewModel.FormatedCookingTime = this.stringFormatService
                .DisplayTime(recipeServiceModel.CookingTime);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            recipeViewModel.UserFavoritedCurrentRecipe = await this.userFavoriteRecipeService
                .ContainsByUserIdAndRecipeId(userId, id);
            recipeViewModel.UserCookedCurrentRecipe = await this.userCookedRecipeService
                .ContainsByUserIdAndRecipeId(userId, id);

            return this.View(recipeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserFavoriteRecipes(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userFavoriteRecipeService.Remove(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [HttpGet]
        public async Task<IActionResult> AddToCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserCookedRecipes(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userCookedRecipeService.Remove(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }
    }
}
