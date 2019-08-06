namespace CookWithMe.Web.Controllers
{
    using System.Collections.Generic;
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
        public async Task<IActionResult> Details(string id)
        {
            var shoppingListServiceModel = await this.shoppingListService.GetById(id);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // TODO: Make this a separate action
            var checkIfUserHasShoppingList = await this.userShoppingListService
                .ContainsByUserIdAndShoppingListId(userId, shoppingListServiceModel.Id);

            if (!checkIfUserHasShoppingList)
            {
                await this.userService
                    .SetShoppingList(userId, shoppingListServiceModel);
            }

            var shoppingListViewModel = shoppingListServiceModel.To<ShoppingListDetailsViewModel>();

            shoppingListViewModel.RecipeTitle = (await this.recipeService.GetById(shoppingListServiceModel.RecipeId)).Title;
            shoppingListViewModel.IngredientsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(shoppingListServiceModel.Ingredients);

            return this.View(shoppingListViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userShoppingListService.Remove(userId, id);

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingListIds = await this.userShoppingListService.GetUserShoppingListIds(userId);
            var shoppingLists = await this.shoppingListService
                .GetAllByIds(shoppingListIds)
                .To<ShoppingListAllViewModel>()
                .ToListAsync();

            return this.View(shoppingLists);
        }
    }
}
