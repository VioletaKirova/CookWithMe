namespace CookWithMe.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.ShoppingLists;

    public interface IUserShoppingListService
    {
        Task<bool> ContainsByUserIdAndShoppingListIdAsync(string userId, string shoppingListId);

        Task<bool> DeleteByUserIdAndShoppingListIdAsync(string userId, string shoppingListId);

        Task<IEnumerable<ShoppingListServiceModel>> GetShoppingListsByUserIdAsync(string userId);

        Task<bool> DeleteByShoppingListIdAsync(string shoppingListId);
    }
}
