namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Categories;
    using CookWithMe.Web.ViewModels.Categories.Delete;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : AdministrationController
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
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateInputModel categoryCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var categoryServiceModel = categoryCreateInputModel.To<CategoryServiceModel>();

            await this.categoryService.CreateAsync(categoryServiceModel);

            var categoryId = await this.categoryService.GetIdByTitleAsync(categoryServiceModel.Title);

            return this.Redirect($"/Categories/Recipes/{categoryId}");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categoryEditInputModel = (await this.categoryService.GetByIdAsync(id))
                .To<CategoryEditInputModel>();

            return this.View(categoryEditInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditInputModel categoryEditInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryEditInputModel);
            }

            var categoryServiceModel = categoryEditInputModel.To<CategoryServiceModel>();

            await this.categoryService.EditAsync(categoryServiceModel);

            return this.Redirect($"/Categories/Recipes/{categoryServiceModel.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var categoryDeleteViewModel = (await this.categoryService.GetByIdAsync(id))
                .To<CategoryDeleteViewModel>();

            return this.View(categoryDeleteViewModel);
        }

        [HttpPost]
        [Route("/Administration/Categories/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var recipeIds = await this.recipeService.GetByCategoryId(id)
                .Select(x => x.Id)
                .ToListAsync();

            foreach (var recipeId in recipeIds)
            {
                await this.recipeService.DeleteByIdAsync(recipeId);
            }

            await this.categoryService.DeleteByIdAsync(id);

            return this.Redirect($"/Categories/Recipes/{GlobalConstants.FirstCategoryId}");
        }
    }
}
