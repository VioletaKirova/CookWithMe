namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IUserService
    {
        Task<bool> AddAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);

        Task<bool> EditAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);

        Task SetUserToRecipe(string userId, Recipe recipe);

        Task<ApplicationUserServiceModel> GetById(string userId);

        Task<bool> SetShoppingList(string userId, ShoppingListServiceModel shoppingListServiceModel);

        Task SetUserToReview(string userId, Review review);

        Task<bool> SetFavoriteRecipe(string userId, Recipe recipe);

        Task<bool> SetCookedRecipe(string userId, Recipe recipe);

        Task<UserAdditionalInfoServiceModel> GetAdditionalInfo(string userId);
    }
}
