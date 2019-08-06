namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class ShoppingListService : IShoppingListService
    {
        private readonly IDeletableEntityRepository<ShoppingList> shoppingListRepository;

        public ShoppingListService(IDeletableEntityRepository<ShoppingList> shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> Delete(string id)
        {
            var shoppingList = await this.shoppingListRepository.GetByIdWithDeletedAsync(id);

            this.shoppingListRepository.Delete(shoppingList);
            var result = await this.shoppingListRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task Edit(string id, ShoppingListServiceModel model)
        {
            var shoppingListFromDb = await this.shoppingListRepository.GetByIdWithDeletedAsync(id);

            shoppingListFromDb.Ingredients = model.Ingredients;

            this.shoppingListRepository.Update(shoppingListFromDb);
        }

        public async Task<ShoppingListServiceModel> GetById(string id)
        {
            var shoppingList = await this.shoppingListRepository
                .GetByIdWithDeletedAsync(id);

            return shoppingList.To<ShoppingListServiceModel>();
        }

        public async Task<string> GetIdByRecipeId(string recipeId)
        {
            var shoppingList = await this.shoppingListRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == recipeId);

            return shoppingList.Id;
        }

        public async Task SetShoppingListToUser(string id, ApplicationUser user)
        {
            user.ShoppingLists.Add(new UserShoppingList
            {
                ShoppingList = await this.shoppingListRepository.GetByIdWithDeletedAsync(id),
            });
        }
    }
}
