namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.ViewModels.ShoppingLists;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ShoppingListsController : BaseController
    {
        private readonly IShoppingListService shoppingListService;
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IStringFormatService stringFormatService;

        public ShoppingListsController(
            IShoppingListService shoppingListService,
            IRecipeService recipeService,
            IUserService userService,
            IStringFormatService stringFormatService)
        {
            this.shoppingListService = shoppingListService;
            this.recipeService = recipeService;
            this.userService = userService;
            this.stringFormatService = stringFormatService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var shoppingListServiceModel = await this.shoppingListService.GetById(id);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var checkIfUserHasShoppingList = this.userService.CheckIfUserHasShoppingList(userId, shoppingListServiceModel.Id);

            if (!checkIfUserHasShoppingList)
            {
                await this.userService
                    .GetShoppingList(userId, shoppingListServiceModel);
            }

            var shoppingListViewModel = shoppingListServiceModel.To<ShoppingListDetailsViewModel>();

            shoppingListViewModel.RecipeTitle = (await this.recipeService.GetById(shoppingListServiceModel.RecipeId)).Title;
            shoppingListViewModel.IngredientsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(shoppingListServiceModel.Ingredients);

            return this.View(shoppingListViewModel);
        }
    }
}
