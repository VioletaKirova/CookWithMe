﻿namespace CookWithMe.Services.Data.ShoppingLists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models.ShoppingLists;

    public interface IShoppingListService
    {
        Task<string> GetIdByRecipeIdAsync(string recipeId);

        Task<ShoppingListServiceModel> GetByIdAsync(string id);

        Task SetShoppingListToUserAsync(string id, ApplicationUser user);

        Task EditAsync(string id, ShoppingListServiceModel shoppingListServiceModel);

        Task<bool> DeleteByIdAsync(string id);
    }
}
