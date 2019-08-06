namespace CookWithMe.Web.ViewComponents
{
    using System.Linq;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class FooterLinksViewComponent : ViewComponent
    {
        private readonly ICategoryService categoryService;

        public FooterLinksViewComponent(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = this.categoryService
                .GetAll()
                .To<FooterLinksViewComponentViewModel>()
                .ToList();

            return this.View(viewModel);
        }
    }
}
