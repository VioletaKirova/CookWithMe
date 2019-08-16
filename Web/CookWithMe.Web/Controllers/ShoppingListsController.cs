namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.Filters;
    using CookWithMe.Web.ViewModels.ShoppingLists.All;
    using CookWithMe.Web.ViewModels.ShoppingLists.Details;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ShoppingListsController : BaseController
    {
        private const string GetErrorMessage = "Failed to get the shopping list.";
        private const string DeleteErrorMessage = "Failed to delete the shopping list.";

        private readonly IShoppingListService shoppingListService;
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IUserShoppingListService userShoppingListService;
        private readonly IStringFormatService stringFormatService;

        public ShoppingListsController(
            IShoppingListService shoppingListService,
            IRecipeService recipeService,
            IUserService userService,
            IUserShoppingListService userShoppingListService,
            IStringFormatService stringFormatService)
        {
            this.shoppingListService = shoppingListService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.userShoppingListService = userShoppingListService;
            this.stringFormatService = stringFormatService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string id)
        {
            var shoppingListServiceModel = await this.shoppingListService.GetByIdAsync(id);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var checkIfUserHasShoppingList = await this.userShoppingListService
                .ContainsByUserIdAndShoppingListIdAsync(userId, shoppingListServiceModel.Id);

            if (!checkIfUserHasShoppingList)
            {
                if (!await this.userService.SetShoppingListAsync(userId, shoppingListServiceModel))
                {
                    this.TempData["Error"] = GetErrorMessage;
                }
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        [ServiceFilter(typeof(ArgumentNullExceptionFilterAttribute))]
        public async Task<IActionResult> Details(string id)
        {
            var shoppingListServiceModel = await this.shoppingListService.GetByIdAsync(id);

            var shoppingListDetailsViewModel = shoppingListServiceModel.To<ShoppingListDetailsViewModel>();

            shoppingListDetailsViewModel.RecipeTitle = (await this.recipeService
                .GetByIdAsync(shoppingListServiceModel.RecipeId)).Title;
            shoppingListDetailsViewModel.IngredientsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(shoppingListServiceModel.Ingredients);

            return this.View(shoppingListDetailsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.userShoppingListService.DeleteByUserIdAndShoppingListIdAsync(userId, id))
            {
                this.TempData["Error"] = DeleteErrorMessage;

                return this.Redirect($"/ShoppingLists/Details/{id}");
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingListIds = await this.userShoppingListService.GetShoppingListIdsByUserIdAsync(userId);
            var shoppingLists = (await this.shoppingListService
                .GetByIds(shoppingListIds))
                .To<ShoppingListAllViewModel>();

            return this.View(shoppingLists);
        }
    }
}
