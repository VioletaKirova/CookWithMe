namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;
    using CookWithMe.Web.InputModels.Administrators.Register;
    using CookWithMe.Web.ViewModels.Administrators.All;

    using Microsoft.AspNetCore.Mvc;

    public class AdministratorsController : AdministrationController
    {
        private const string RegistrationErrorMessage = "The administrator registration failed.";
        private const string RemoveFromRoleErrorMessage = "Failed to remove administrator from role.";
        private const string RemoveFromRoleSuccessMessage = "You successfully removed administrator from role!";

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

            await this.administratorService.RegisterAsync(administratorRegisterServiceModel);

            if (!await this.administratorService.RegisterAsync(administratorRegisterServiceModel))
            {
                this.TempData["Error"] = RegistrationErrorMessage;

                return this.View();
            }

            return this.RedirectToAction(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromRole(string id)
        {
            if (!await this.administratorService.RemoveFromRoleByIdAsync(id))
            {
                this.TempData["Error"] = RemoveFromRoleErrorMessage;

                return this.RedirectToAction(nameof(this.All));
            }

            this.TempData["Success"] = RemoveFromRoleSuccessMessage;

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
