namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class UserShoppingListService : IUserShoppingListService
    {
        private readonly IRepository<UserShoppingList> userShoppingListRepository;

        public UserShoppingListService(IRepository<UserShoppingList> userShoppingListRepository)
        {
            this.userShoppingListRepository = userShoppingListRepository;
        }

        public async Task<bool> ContainsByUserIdAndShoppingListId(string userId, string shoppingListId)
        {
            var userShoppingListsIds = await this.userShoppingListRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(s => s.ShoppingListId)
               .ToListAsync();

            return userShoppingListsIds.Contains(shoppingListId);
        }

        public async Task<bool> Remove(string userId, string shoppingListId)
        {
            var userShoppingList = await this.userShoppingListRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.ShoppingListId == shoppingListId);

            this.userShoppingListRepository.Delete(userShoppingList);

            var result = await this.userShoppingListRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
