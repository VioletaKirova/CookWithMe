namespace CookWithMe.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.Filters;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.ViewModels.Categories.Filtered;
    using CookWithMe.Web.ViewModels.Categories.Recipes;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IRecipeService recipeService;

        public CategoriesController(
            ICategoryService categoryService,
            IRecipeService recipeService)
        {
            this.categoryService = categoryService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        [ServiceFilter(typeof(ArgumentNullExceptionFilterAttribute))]
        public async Task<IActionResult> Recipes(int id, int? pageNumber)
        {
            this.ViewData["CategoryTitle"] = (await this.categoryService.GetByIdAsync(id)).Title;
            this.ViewData["CategoryId"] = id;

            var recipesFromCategory = this.recipeService.GetByCategoryId(id)
                .To<CategoryRecipesRecipeViewModel>();

            return this.View(await PaginatedList<CategoryRecipesRecipeViewModel>
                .CreateAsync(recipesFromCategory, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        [ServiceFilter(typeof(ArgumentNullExceptionFilterAttribute))]
        public async Task<IActionResult> Filtered(int id, int? pageNumber)
        {
            this.ViewData["CategoryTitle"] = (await this.categoryService.GetByIdAsync(id)).Title;
            this.ViewData["CategoryId"] = id;

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var filteredRecipesFromCategory = (await this.recipeService.GetAllFilteredAsync(userId))
                .Where(x => x.CategoryId == id)
                .To<CategoryFilteredRecipeViewModel>();

            return this.View(await PaginatedList<CategoryFilteredRecipeViewModel>
                .CreateAsync(filteredRecipesFromCategory, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }
    }
}
