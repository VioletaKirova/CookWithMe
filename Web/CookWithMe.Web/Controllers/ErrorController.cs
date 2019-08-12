namespace CookWithMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using CookWithMe.Common;
    using CookWithMe.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        [Route("/Error/500")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult InternalServerError()
        {
            var errorViewModel = new ErrorViewModel
            {
                StatusCode = StatusCodes.InternalServerError,
                RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
            };

            return this.View(errorViewModel);
        }

        [Route("/Error/404")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult NotFoundError()
        {
            var errorViewModel = new ErrorViewModel();

            if (this.TempData["ErrorParams"] is Dictionary<string, string> dict)
            {
                errorViewModel.StatusCode = StatusCodes.NotFound;
                errorViewModel.RequestId = dict["RequestId"];
                errorViewModel.RequestPath = dict["RequestPath"];
            }

            return this.View(errorViewModel);
        }
    }
}
