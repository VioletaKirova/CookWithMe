namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<Allergen> allergenRepository;
        private readonly IRepository<UserAllergen> userAllergenRepository;
        private readonly IRepository<Lifestyle> lifestyleRepository;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IRepository<Allergen> allergenRepository,
            IRepository<UserAllergen> userAllergenRepository,
            IRepository<Lifestyle> lifestyleRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.allergenRepository = allergenRepository;
            this.userAllergenRepository = userAllergenRepository;
            this.lifestyleRepository = lifestyleRepository;
        }

        public async Task<bool> UpdateUserAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.Biography = additionalInfoServiceModel.Biography;
            user.ProfilePhoto = additionalInfoServiceModel.ProfilePhoto;
            user.Lifestyle = await this.lifestyleRepository.All().SingleOrDefaultAsync(x => x.Type == additionalInfoServiceModel.LifestyleType);

            foreach (var allergenName in additionalInfoServiceModel.Allergies)
            {
                await this.userAllergenRepository.AddAsync(new UserAllergen
                {
                    User = user,
                    Allergen = await this.allergenRepository.All().SingleOrDefaultAsync(x => x.Name == allergenName),
                });
            }

            await this.userAllergenRepository.SaveChangesAsync();
            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
