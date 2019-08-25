namespace CookWithMe.Web.ViewComponents
{
    using System.Linq;

    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class FilteredCategorySidebarViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public FilteredCategorySidebarViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.categoryService
                .GetAll()
                .OrderBy(x => x.Id)
                .To<FilteredCategorySidebarViewComponentViewModel>()
                .ToList();

            return this.View(viewModel);
        }
    }
}
