namespace CookWithMe.Services.Data.Administrators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Administrators;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;

    public class AdministratorService : IAdministratorService
    {
        private const string InvalidUserIdErrorMessage = "User with ID: {0} does not exist.";

        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public AdministratorService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<AdministratorServiceModel>> GetAllAsync()
        {
            return (await this.userManager.
                GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName))
                .Where(x => x.UserName != this.configuration["Root:UserName"])
                .To<AdministratorServiceModel>();
        }

        public async Task<bool> RegisterAsync(AdministratorServiceModel administratorServiceModel)
        {
            var user = administratorServiceModel.To<ApplicationUser>();

            var result = await this.userManager.CreateAsync(
                user,
                administratorServiceModel.Password);

            if (result.Succeeded)
            {
                result = await this.userManager.AddToRoleAsync(
                    user,
                    GlobalConstants.AdministratorRoleName);
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveFromRoleByIdAsync(string userId)
        {
            var administrator = await this.userRepository
                .GetByIdWithDeletedAsync(userId);

            if (administrator == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidUserIdErrorMessage, userId));
            }

            var result = await this.userManager.RemoveFromRoleAsync(
                administrator,
                GlobalConstants.AdministratorRoleName);

            if (result.Succeeded)
            {
                result = await this.userManager.AddToRoleAsync(
                    administrator,
                    GlobalConstants.UserRoleName);
            }

            return result.Succeeded;
        }

        public async Task<bool> IsInAdministratorRoleAsync(string userId)
        {
            var user = await this.userRepository
                .GetByIdWithDeletedAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidUserIdErrorMessage, userId));
            }

            return await this.userManager
                .IsInRoleAsync(user, GlobalConstants.AdministratorRoleName);
        }
    }
}
