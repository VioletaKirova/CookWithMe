namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Administrators;

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

        [HttpPost]
        public async Task<IActionResult> Register(AdministratorRegisterInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var administrator = AutoMapper.Mapper.Map<AdministratorRegisterInputModel, AdministratorServiceModel>(model);

            await this.administratorService.RegisterAsync(administrator);

            return this.Redirect("/");
        }
    }
}
