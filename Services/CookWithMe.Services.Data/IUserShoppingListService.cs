namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IUserShoppingListService
    {
        Task<bool> ContainsByUserIdAndShoppingListId(string userId, string shoppingListId);

        Task<bool> Remove(string userId, string shoppingListId);

        Task<IEnumerable<string>> GetUserShoppingListIds(string userId);

        Task<bool> DeleteByShoppingListId(string shoppingListId);
    }
}
