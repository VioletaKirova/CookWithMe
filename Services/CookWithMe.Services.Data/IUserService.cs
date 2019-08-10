namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IUserService
    {
        Task<bool> AddAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);

        Task<bool> EditAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);

        Task SetUserToRecipeAsync(string userId, Recipe recipe);

        Task<ApplicationUserServiceModel> GetByIdAsync(string userId);

        Task<bool> SetShoppingListAsync(string userId, ShoppingListServiceModel shoppingListServiceModel);

        Task SetUserToReviewAsync(string userId, Review review);

        Task<bool> SetFavoriteRecipeAsync(string userId, Recipe recipe);

        Task<bool> SetCookedRecipeAsync(string userId, Recipe recipe);

        Task<UserAdditionalInfoServiceModel> GetAdditionalInfoByUserIdAsync(string userId);
    }
}
