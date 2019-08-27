namespace CookWithMe.Services.Data.Users
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models.ShoppingLists;
    using CookWithMe.Services.Models.Users;

    public interface IUserService
    {
        Task<bool> AddAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel userAdditionalInfoServiceModel);

        Task<bool> EditAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel userAdditionalInfoServiceModel);

        Task SetUserToRecipeAsync(string userId, Recipe recipe);

        Task<ApplicationUserServiceModel> GetByIdAsync(string userId);

        Task<bool> SetShoppingListAsync(string userId, ShoppingListServiceModel shoppingListServiceModel);

        Task SetUserToReviewAsync(string userId, Review review);

        Task<bool> SetFavoriteRecipeAsync(string userId, Recipe recipe);

        Task<bool> SetCookedRecipeAsync(string userId, Recipe recipe);

        Task<UserAdditionalInfoServiceModel> GetAdditionalInfoByUserIdAsync(string userId);

        Task<string> GetIdByUserNameAsync(string userName);
    }
}
