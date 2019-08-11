namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;
    using CookWithMe.Web.InputModels.Administrators.Register;
    using CookWithMe.Web.ViewModels.Administrators.All;

    using Microsoft.AspNetCore.Mvc;

    public class AdministratorsController : AdministrationController
    {
        private readonly IAdministratorService administratorService;

        public AdministratorsController(IAdministratorService administratorService)
        {
            this.administratorService = administratorService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var administrators = (await this.administratorService.GetAllAsync())
                .To<AdministratorAllViewModel>();

            return this.View(administrators);
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdministratorRegisterInputModel administratorRegisterInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var administratorRegisterServiceModel = administratorRegisterInputModel
                .To<AdministratorServiceModel>();

            if (!await this.administratorService.RegisterAsync(administratorRegisterServiceModel))
            {
                return this.Redirect($"/Home/Error?statusCode={StatusCodes.InternalServerError}&id={this.HttpContext.TraceIdentifier}");
            }

            return this.RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromRole(string id)
        {
            await this.administratorService.RemoveFromRoleByIdAsync(id);

            return this.RedirectToAction("All");
        }
    }
}
