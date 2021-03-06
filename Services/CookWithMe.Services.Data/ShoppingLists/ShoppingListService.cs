﻿namespace CookWithMe.Services.Data.ShoppingLists
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

    public class ShoppingListService : IShoppingListService
    {
        private const string InvalidShoppingListIdErrorMessage = "ShoppingList with ID: {0} does not exist.";
        private const string InvalidRecipeIdErrorMessage = "ShoppingList with RecipeId: {0} does not exist.";

        private readonly IDeletableEntityRepository<ShoppingList> shoppingListRepository;

        public ShoppingListService(IDeletableEntityRepository<ShoppingList> shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var shoppingListFromDb = await this.shoppingListRepository
                .GetByIdWithDeletedAsync(id);

            if (shoppingListFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidShoppingListIdErrorMessage, id));
            }

            this.shoppingListRepository.Delete(shoppingListFromDb);
            var result = await this.shoppingListRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task EditAsync(string id, ShoppingListServiceModel shoppingListServiceModel)
        {
            var shoppingListFromDb = await this.shoppingListRepository
                .GetByIdWithDeletedAsync(id);

            if (shoppingListFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidShoppingListIdErrorMessage, id));
            }

            shoppingListFromDb.Ingredients = shoppingListServiceModel.Ingredients;

            this.shoppingListRepository.Update(shoppingListFromDb);
        }

        public async Task<ShoppingListServiceModel> GetByIdAsync(string id)
        {
            var shoppingList = await this.shoppingListRepository
                .GetByIdWithDeletedAsync(id);

            if (shoppingList == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidShoppingListIdErrorMessage, id));
            }

            return shoppingList.To<ShoppingListServiceModel>();
        }

        public async Task<string> GetIdByRecipeIdAsync(string recipeId)
        {
            var shoppingList = await this.shoppingListRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == recipeId);

            if (shoppingList == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, recipeId));
            }

            return shoppingList.Id;
        }

        public async Task SetShoppingListToUserAsync(string id, ApplicationUser user)
        {
            user.ShoppingLists.Add(new UserShoppingList
            {
                ShoppingList = await this.shoppingListRepository
                    .GetByIdWithDeletedAsync(id),
            });
        }
    }
}
