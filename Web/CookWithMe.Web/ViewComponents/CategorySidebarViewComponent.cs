namespace CookWithMe.Web.ViewComponents
{
    using System.Linq;

    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class CategorySidebarViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public CategorySidebarViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.categoryService
                .GetAll()
                .To<CategorySidebarViewComponentViewModel>()
                .ToList();

            return this.View(viewModel);
        }
    }
}
