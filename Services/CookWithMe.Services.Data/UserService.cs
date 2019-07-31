namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly ILifestyleService lifestyleService;
        private readonly IAllergenService allergenService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ILifestyleService lifestyleService,
            IAllergenService allergenService)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.lifestyleService = lifestyleService;
            this.allergenService = allergenService;
        }

        public async Task<ApplicationUserServiceModel> GetById(string userId)
        {
            var userFromDb = await this.userRepository.GetByIdWithDeletedAsync(userId);
            var userServiceModel = AutoMapper.Mapper.Map<ApplicationUserServiceModel>(userFromDb);

            return userServiceModel;
        }

        public async Task SetUserToRecipe(string userId, Recipe recipe)
        {
            recipe.User = await this.userRepository.GetByIdWithDeletedAsync(userId);
        }

        public async Task<bool> UpdateUserAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.Biography = additionalInfoServiceModel.Biography;
            user.ProfilePhoto = additionalInfoServiceModel.ProfilePhoto;

            if (additionalInfoServiceModel.Lifestyle != null)
            {
                await this.lifestyleService.SetLifestyleToUser(additionalInfoServiceModel.Lifestyle.Type, user);
            }

            if (additionalInfoServiceModel.Allergies != null)
            {
                foreach (var userAllergen in additionalInfoServiceModel.Allergies)
                {
                    await this.allergenService.SetAllergenToUser(userAllergen.Allergen.Name, user);
                }
            }

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
