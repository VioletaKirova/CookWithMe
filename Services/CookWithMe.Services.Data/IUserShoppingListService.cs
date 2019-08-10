namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserShoppingListService
    {
        Task<bool> ContainsByUserIdAndShoppingListIdAsync(string userId, string shoppingListId);

        Task<bool> DeleteByUserIdAndShoppingListIdAsync(string userId, string shoppingListId);

        Task<IEnumerable<string>> GetShoppingListIdsByUserIdAsync(string userId);

        Task<bool> DeleteByShoppingListIdAsync(string shoppingListId);
    }
}
