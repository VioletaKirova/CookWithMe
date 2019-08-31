namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;
    using CookWithMe.Web.Filters;
    using CookWithMe.Web.InputModels.Administrators.Register;
    using CookWithMe.Web.ViewModels.Administrators.All;

    using Microsoft.AspNetCore.Mvc;

    [ServiceFilter(typeof(AuthorizeRootUserFilterAttribute))]
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
        public async Task<IActionResult> All()
        {
            var administrators = (await this.administratorService.GetAllAsync())
                .OrderBy(x => x.FullName)
                .To<AdministratorAllViewModel>();

            return this.View(administrators);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdministratorRegisterInputModel administratorRegisterInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(administratorRegisterInputModel);
            }

            var administratorRegisterServiceModel = administratorRegisterInputModel
                .To<AdministratorServiceModel>();

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
