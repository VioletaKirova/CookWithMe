namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly ICategoryService categoryService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly IShoppingListService shoppingListService;
        private readonly INutritionalValueService nutritionalValueService;

        public RecipeService(
            IDeletableEntityRepository<Recipe> recipeRepository,
            ICategoryService categoryService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            IShoppingListService shoppingListService, 
            INutritionalValueService nutritionalValueService)
        {
            this.recipeRepository = recipeRepository;
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.shoppingListService = shoppingListService;
            this.nutritionalValueService = nutritionalValueService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel model)
        {
            var recipe = AutoMapper.Mapper.Map<RecipeServiceModel, Recipe>(model);

            await this.categoryService.SetCategoryToRecipe(model.CategoryTitle, recipe);
            await this.lifestyleService.SetLifestyleToRecipe(model.LifestyleType, recipe);

            foreach (var allergenName in model.AllergenNames)
            {
                await this.allergenService.SetAllergenToRecipe(allergenName, recipe);
            }

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
