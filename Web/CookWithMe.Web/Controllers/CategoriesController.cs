namespace CookWithMe.Web.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.ViewModels.Categories.Recipes;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;
        private readonly IRecipeService recipeService;

        public CategoriesController(ICategoryService categoryService, IRecipeService recipeService)
        {
            this.categoryService = categoryService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> Recipes(int id, int? pageNumber)
        {
            this.ViewData["CategoryId"] = id;
            this.ViewData["CategoryTitle"] = (await this.categoryService.GetByIdAsync(id)).Title;

            var recipesFromCategory = this.recipeService.GetByCategoryId(id)
                .To<CategoryRecipesRecipeViewModel>();

            int pageSize = GlobalConstants.PageSize;
            return this.View(await PaginatedList<CategoryRecipesRecipeViewModel>.CreateAsync(recipesFromCategory, pageNumber ?? 1, pageSize));
        }
    }
}
