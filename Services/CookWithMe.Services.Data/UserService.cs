namespace CookWithMe.Services.Data
{
    using System.Linq;
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
        private readonly IShoppingListService shoppingListService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ILifestyleService lifestyleService,
            IAllergenService allergenService,
            IShoppingListService shoppingListService)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
            this.lifestyleService = lifestyleService;
            this.allergenService = allergenService;
            this.shoppingListService = shoppingListService;
        }

        public async Task<bool> SetShoppingList(string userId, ShoppingListServiceModel shoppingListServiceModel)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);
            await this.shoppingListService.SetShoppingListToUser(shoppingListServiceModel.Id, user);

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<ApplicationUserServiceModel> GetById(string userId)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);
            var userServiceModel = user.To<ApplicationUserServiceModel>();

            return userServiceModel;
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

        public async Task SetUserToReview(string userId, Review review)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            review.Reviewer = user;
        }

        public async Task SetUserToRecipe(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            recipe.User = user;
        }

        public async Task<bool> SetFavoriteRecipe(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.FavoriteRecipes.Add(new UserFavoriteRecipe
            {
                Recipe = recipe,
            });

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> SetCookedRecipe(string userId, Recipe recipe)
        {
            var user = await this.userRepository.GetByIdWithDeletedAsync(userId);

            user.CookedRecipes.Add(new UserCookedRecipe
            {
                Recipe = recipe,
            });

            var result = await this.userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
