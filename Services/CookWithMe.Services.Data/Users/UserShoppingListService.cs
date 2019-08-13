namespace CookWithMe.Services.Data.Users
{
    using System.Collections.Generic;
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

        public async Task<bool> ContainsByUserIdAndShoppingListIdAsync(string userId, string shoppingListId)
        {
            var userShoppingListsIds = await this.userShoppingListRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(s => s.ShoppingListId)
               .ToListAsync();

            return userShoppingListsIds.Contains(shoppingListId);
        }

        public async Task<bool> DeleteByShoppingListIdAsync(string shoppingListId)
        {
            var userShoppingLists = this.userShoppingListRepository
                .All()
                .Where(x => x.ShoppingListId == shoppingListId);

            if (userShoppingLists.Any())
            {
                foreach (var userShoppingList in userShoppingLists)
                {
                    this.userShoppingListRepository.Delete(userShoppingList);
                }
            }

            var result = await this.userShoppingListRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<string>> GetShoppingListIdsByUserIdAsync(string userId)
        {
            return await this.userShoppingListRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Select(x => x.ShoppingListId)
                .ToListAsync();
        }

        public async Task<bool> DeleteByUserIdAndShoppingListIdAsync(string userId, string shoppingListId)
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
