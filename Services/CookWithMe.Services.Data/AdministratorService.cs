namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Identity;

    public class AdministratorService : IAdministratorService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdministratorService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }


        public async Task<bool> RegisterAsync(AdministratorServiceModel model)
        {
            var user = AutoMapper.Mapper.Map<AdministratorServiceModel, ApplicationUser>(model);

            var result = await this.userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                result = await this.userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
            }

            return result.Succeeded;
        }
    }
}
