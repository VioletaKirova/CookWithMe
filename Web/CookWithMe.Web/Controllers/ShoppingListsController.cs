namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.ShoppingLists;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ShoppingListsController : BaseController
    {
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
                await this.userService
                    .SetShoppingListAsync(userId, shoppingListServiceModel);
            }

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var shoppingListServiceModel = await this.shoppingListService.GetByIdAsync(id);

            var shoppingListViewModel = shoppingListServiceModel.To<ShoppingListDetailsViewModel>();

            shoppingListViewModel.RecipeTitle = (await this.recipeService.GetByIdAsync(shoppingListServiceModel.RecipeId)).Title;
            shoppingListViewModel.IngredientsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(shoppingListServiceModel.Ingredients);

            return this.View(shoppingListViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userShoppingListService.DeleteByUserIdAndShoppingListIdAsync(userId, id);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingListIds = await this.userShoppingListService.GetShoppingListIdsByUserIdAsync(userId);
            var shoppingLists = await this.shoppingListService
                .GetByIdsAsync(shoppingListIds)
                .To<ShoppingListAllViewModel>()
                .ToListAsync();

            return this.View(shoppingLists);
        }
    }
}
