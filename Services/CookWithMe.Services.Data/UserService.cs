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
        private readonly IRepository<UserAllergen> userAllergensRepository;

        public UserService(UserManager<ApplicationUser> userManager, IDeletableEntityRepository<ApplicationUser> userRepository, IRepository<UserAllergen> userAllergensRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.userAllergensRepository = userAllergensRepository;
        }

        public async Task<bool> UpdateUserAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.Biography = additionalInfoServiceModel.Biography;
            user.ProfilePhoto = additionalInfoServiceModel.ProfilePhoto;
            user.LifestyleId = additionalInfoServiceModel.LifestyleId;

            foreach (var userAllergen in additionalInfoServiceModel.Allergies)
            {
                await this.userAllergensRepository.AddAsync(new UserAllergen
                {
                    UserId = userAllergen.UserId,
                    AllergenId = userAllergen.AllergenId,
                });
            }

            await this.userAllergensRepository.SaveChangesAsync();
            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
