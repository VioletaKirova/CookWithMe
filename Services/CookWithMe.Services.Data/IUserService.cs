namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IUserService
    {
        Task<bool> UpdateUserAdditionalInfoAsync(string userId, UserAdditionalInfoServiceModel additionalInfoServiceModel);

        Task SetUserToRecipe(string userId, Recipe recipe);

        Task<ApplicationUserServiceModel> GetById(string userId);

        Task<bool> GetShoppingList(string userId, ShoppingListServiceModel shoppingListServiceModel);

        bool CheckIfUserHasShoppingList(string userId, string shoppingListId);

        Task SetUserToReview(string userId, Review review);
    }
}
