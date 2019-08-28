namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;
    using CookWithMe.Web.InputModels.Categories.Create;
    using CookWithMe.Web.InputModels.Categories.Edit;
    using CookWithMe.Web.ViewModels.Categories.Delete;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : AdministrationController
    {
        private const string CreateErrorMessage = "Failed to create the category.";
        private const string EditErrorMessage = "Failed to edit the category.";
        private const string DeleteErrorMessage = "Failed to delete the category.";
        private const string DeleteSuccessMessage = "You successfully deleted {0} Category!";

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

            if (!await this.categoryService.CreateAsync(categoryServiceModel))
            {
                this.TempData["Error"] = CreateErrorMessage;

                return this.View();
            }

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
                return this.View();
            }

            var categoryServiceModel = categoryEditInputModel.To<CategoryServiceModel>();

            if (!await this.categoryService.EditAsync(categoryServiceModel))
            {
                this.TempData["Error"] = EditErrorMessage;

                return this.View();
            }

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

            var categoryTitle = (await this.categoryService.GetByIdAsync(id)).Title;

            if (!await this.categoryService.DeleteByIdAsync(id))
            {
                this.TempData["Error"] = DeleteErrorMessage;

                return this.View();
            }

            this.TempData["Success"] = string.Format(DeleteSuccessMessage, categoryTitle);

            return this.Redirect($"/Categories/Recipes/{GlobalConstants.DefaultCategoryId}");
        }
    }
}
