namespace CookWithMe.Web.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Categories.All;

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
        public async Task<IActionResult> All(int id)
        {
            var categoryViewModel = (await this.categoryService.GetById(id))
                .To<CategoryAllViewModel>();
            categoryViewModel.Recipes = this.recipeService.GetAllByCategoryId(id)
                .To<CategoryAllRecipeViewModel>();

            return this.View(categoryViewModel);
        }
    }
}
