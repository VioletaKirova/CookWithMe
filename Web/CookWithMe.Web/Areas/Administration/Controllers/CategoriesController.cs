namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
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
        public async Task<IActionResult> Create(CategoryCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var serviceModel = AutoMapper.Mapper.Map<CategoryCreateInputModel, CategoryServiceModel>(model);

            await this.categoryService.CreateAsync(serviceModel);

            return this.Redirect("/");
        }
    }
}
