namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IShoppingListService
    {
        Task<string> GetIdByRecipeId(string recipeId);

        Task<ShoppingListServiceModel> GetById(string id);
    }
}
