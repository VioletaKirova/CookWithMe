namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IShoppingListService
    {
        Task<string> GetIdByRecipeIdAsync(string recipeId);

        Task<ShoppingListServiceModel> GetByIdAsync(string id);

        IQueryable<ShoppingListServiceModel> GetByIdsAsync(IEnumerable<string> ids);

        Task SetShoppingListToUserAsync(string id, ApplicationUser user);

        Task EditAsync(string id, ShoppingListServiceModel shoppingListServiceModel);

        Task<bool> DeleteByIdAsync(string id);
    }
}
