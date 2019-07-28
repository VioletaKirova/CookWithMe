namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly IShoppingListService shoppingListService;

        public RecipeService(IDeletableEntityRepository<Recipe> recipeRepository, IShoppingListService shoppingListService)
        {
            this.recipeRepository = recipeRepository;
            this.shoppingListService = shoppingListService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel model)
        {
            var recipe = AutoMapper.Mapper.Map<RecipeServiceModel, Recipe>(model);

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
