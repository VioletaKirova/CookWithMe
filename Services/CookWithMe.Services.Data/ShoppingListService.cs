namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class ShoppingListService : IShoppingListService
    {
        private readonly IDeletableEntityRepository<ShoppingList> shoppingListRepository;

        public ShoppingListService(IDeletableEntityRepository<ShoppingList> shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<string> GetIdByRecipeId(string recipeId)
        {
            var shoppingList = await this.shoppingListRepository.AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == recipeId);

            return shoppingList.Id;
        }
    }
}
