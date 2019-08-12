namespace CookWithMe.Web.ViewComponents
{
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ErrorAlertViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string message)
        {
            var viewModel = new ErrorAlertViewComponentViewModel()
            {
                ErrorMessage = message,
            };

            return this.View(viewModel);
        }
    }
}
