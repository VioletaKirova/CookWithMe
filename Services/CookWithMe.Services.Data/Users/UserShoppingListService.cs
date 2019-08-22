namespace CookWithMe.Services.Data.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    using Microsoft.EntityFrameworkCore;

    public class UserShoppingListService : IUserShoppingListService
    {
        private const string InvalidUserShoppingListErrorMessage = "UserShoppingList with UserId: {0} and ShoppingListId: {1} does not exist.";

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

        public async Task<IEnumerable<ShoppingListServiceModel>> GetShoppingListsByUserIdAsync(string userId)
        {
            return await this.userShoppingListRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.AddedOn)
                .Select(x => x.ShoppingList)
                .To<ShoppingListServiceModel>()
                .ToListAsync();
        }

        public async Task<bool> DeleteByUserIdAndShoppingListIdAsync(string userId, string shoppingListId)
        {
            var userShoppingList = await this.userShoppingListRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.ShoppingListId == shoppingListId);

            if (userShoppingList == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidUserShoppingListErrorMessage, userId, shoppingListId));
            }

            this.userShoppingListRepository.Delete(userShoppingList);

            var result = await this.userShoppingListRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
