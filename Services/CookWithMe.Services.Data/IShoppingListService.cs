namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface IShoppingListService
    {
        Task<string> GetIdByRecipeId(string recipeId);
    }
}
