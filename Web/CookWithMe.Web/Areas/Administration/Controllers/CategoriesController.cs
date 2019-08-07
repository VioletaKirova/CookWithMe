namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

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

        public CategoriesController(ICategoryService categoryService, IRecipeService recipeService)
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
        public async Task<IActionResult> Create(CategoryCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var serviceModel = inputModel.To<CategoryServiceModel>();

            await this.categoryService.CreateAsync(serviceModel);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = (await this.categoryService.GetById(id))
                .To<CategoryEditInputModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var serviceModel = inputModel.To<CategoryServiceModel>();

            await this.categoryService.EditAsync(serviceModel);

            return this.Redirect($"/Categories/All/{serviceModel.Id}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = (await this.categoryService.GetById(id))
                .To<CategoryDeleteViewModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        [Route("/Administration/Categories/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var recipeIds = await this.recipeService.GetAllByCategoryId(id)
                .Select(x => x.Id)
                .ToListAsync();

            foreach (var recipeId in recipeIds)
            {
                await this.recipeService.Delete(recipeId);
            }

            await this.categoryService.DeleteAsync(id);

            return this.Redirect("/");
        }
    }
}
