﻿namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Identity;

    public class AdministratorService : IAdministratorService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public AdministratorService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<AdministratorServiceModel>> GetAllAsync()
        {
            return (await this.userManager.
                GetUsersInRoleAsync(GlobalConstants.AdministratorRoleName))
                .Where(x => x.UserName != GlobalConstants.RootUsername)
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

        public async Task RemoveFromRoleByIdAsync(string userId)
        {
            var administrator = await this.userRepository
                .GetByIdWithDeletedAsync(userId);

            await this.userManager.RemoveFromRoleAsync(
                administrator,
                GlobalConstants.AdministratorRoleName);

            await this.userManager.AddToRoleAsync(
                administrator,
                GlobalConstants.UserRoleName);
        }
    }
}
