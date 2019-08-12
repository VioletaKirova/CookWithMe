namespace CookWithMe.Web.ViewComponents
{
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class SuccessAlertViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string message)
        {
            var viewModel = new SuccessAlertViewComponentViewModel()
            {
                SuccessMessage = message,
            };

            return this.View(viewModel);
        }
    }
}
