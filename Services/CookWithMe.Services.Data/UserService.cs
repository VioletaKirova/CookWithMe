namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly ILifestyleService lifestyleService;
        private readonly IAllergenService allergenService;
        private readonly IUserAllergenService userAllergenService;
        private readonly IShoppingListService shoppingListService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ILifestyleService lifestyleService,
            IAllergenService allergenService,
            IUserAllergenService userAllergenService,
            IShoppingListService shoppingListService)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.lifestyleService = lifestyleService;
            this.allergenService = allergenService;
            this.userAllergenService = userAllergenService;
            this.shoppingListService = shoppingListService;
        }

        public async Task<bool> SetShoppingListAsync(string userId, ShoppingListServiceModel shoppingListServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);
            await this.shoppingListService.SetShoppingListToUserAsync(shoppingListServiceModel.Id, user);

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<ApplicationUserServiceModel> GetByIdAsync(string userId)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);
            var userServiceModel = user.To<ApplicationUserServiceModel>();

            return userServiceModel;
        }

        public async Task<bool> AddAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel userAdditionalInfoServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.HasAdditionalInfo = true;
            user.Biography = userAdditionalInfoServiceModel.Biography;
            user.ProfilePhoto = userAdditionalInfoServiceModel.ProfilePhoto;

            if (userAdditionalInfoServiceModel.Lifestyle != null)
            {
                await this.lifestyleService
                    .SetLifestyleToUserAsync(userAdditionalInfoServiceModel.Lifestyle.Type, user);
            }

            if (userAdditionalInfoServiceModel.Allergies != null)
            {
                foreach (var userAllergen in userAdditionalInfoServiceModel.Allergies)
                {
                    await this.allergenService
                        .SetAllergenToUserAsync(userAllergen.Allergen.Name, user);
                }
            }

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> EditAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel userAdditionalInfoServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.FullName = userAdditionalInfoServiceModel.FullName;
            user.Biography = userAdditionalInfoServiceModel.Biography;
            user.ProfilePhoto = userAdditionalInfoServiceModel.ProfilePhoto;

            if (userAdditionalInfoServiceModel.Lifestyle != null)
            {
                await this.lifestyleService
                    .SetLifestyleToUserAsync(userAdditionalInfoServiceModel.Lifestyle.Type, user);
            }

            this.userAllergenService.DeletePreviousUserAllergensByUserId(userId);

            if (userAdditionalInfoServiceModel.Allergies != null)
            {
                foreach (var userAllergen in userAdditionalInfoServiceModel.Allergies)
                {
                    await this.allergenService
                        .SetAllergenToUserAsync(userAllergen.Allergen.Name, user);
                }
            }

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task SetUserToReviewAsync(string userId, Review review)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            review.Reviewer = user;
        }

        public async Task SetUserToRecipeAsync(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            recipe.User = user;
        }

        public async Task<bool> SetFavoriteRecipeAsync(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.FavoriteRecipes.Add(new UserFavoriteRecipe
            {
                Recipe = recipe,
            });

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> SetCookedRecipeAsync(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.CookedRecipes.Add(new UserCookedRecipe
            {
                Recipe = recipe,
            });

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<UserAdditionalInfoServiceModel> GetAdditionalInfoByUserIdAsync(string userId)
        {
            var additionalInfoServiceModel = (await this.userRepository
                .GetByIdWithDeletedAsync(userId))
                .To<UserAdditionalInfoServiceModel>();

            if (additionalInfoServiceModel.LifestyleId != null)
            {
                additionalInfoServiceModel.Lifestyle = await this.lifestyleService
                    .GetByIdAsync(additionalInfoServiceModel.LifestyleId.Value);
            }

            additionalInfoServiceModel.Allergies = await this.userAllergenService
                .GetByUserIdAsync(userId);

            return additionalInfoServiceModel;
        }
    }
}
