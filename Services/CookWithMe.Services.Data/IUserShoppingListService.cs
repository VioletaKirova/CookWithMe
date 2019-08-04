namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserShoppingListService
    {
        Task<bool> ContainsByUserIdAndShoppingListId(string userId, string shoppingListId);

        Task<bool> Remove(string userId, string shoppingListId);
    }
}
