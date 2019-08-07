namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Categories;

    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
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
    }
}
