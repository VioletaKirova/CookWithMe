namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public interface IShoppingListService
    {
        Task<string> GetIdByRecipeId(string recipeId);

        Task<ShoppingListServiceModel> GetById(string id);

        Task SetShoppingListToUser(string id, ApplicationUser user);

        Task Edit(string id, ShoppingListServiceModel model);

        Task<bool> Delete(string id);
    }
}
