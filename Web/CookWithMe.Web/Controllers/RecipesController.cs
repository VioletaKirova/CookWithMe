namespace CookWithMe.Web.Controllers
{
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
        private readonly IStringFormatService stringFormatService;

        public RecipesController(IRecipeService recipeService, IStringFormatService stringFormatService)
        {
            this.recipeService = recipeService;
            this.stringFormatService = stringFormatService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var recipeServiceModel = await this.recipeService.GetById(id);
            var recipeViewModel = recipeServiceModel.To<RecipeDetailsViewModel>();

            recipeViewModel.DirectionsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.Directions);

            recipeViewModel.ShoppingListIngredients = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.ShoppingList.Ingredients);

            return this.View(recipeViewModel);
        }
    }
}
