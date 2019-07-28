namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IRepository<Allergen> allergenRepository;
        private readonly IRepository<Lifestyle> lifestyleRepository;
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly IShoppingListService shoppingListService;
        private readonly INutritionalValueService nutritionalValueService;

        public RecipeService(
            IDeletableEntityRepository<Category> categoryRepository,
            IRepository<Allergen> allergenRepository,
            IRepository<Lifestyle> lifestyleRepository,
            IDeletableEntityRepository<Recipe> recipeRepository,
            IShoppingListService shoppingListService,
            INutritionalValueService nutritionalValueService)
        {
            this.categoryRepository = categoryRepository;
            this.allergenRepository = allergenRepository;
            this.lifestyleRepository = lifestyleRepository;
            this.recipeRepository = recipeRepository;
            this.shoppingListService = shoppingListService;
            this.nutritionalValueService = nutritionalValueService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel model)
        {
            var recipe = AutoMapper.Mapper.Map<RecipeServiceModel, Recipe>(model);

            recipe.Category = await this.categoryRepository.All().SingleOrDefaultAsync(x => x.Title == model.CategoryTitle);
            recipe.Lifestyle = await this.lifestyleRepository.All().SingleOrDefaultAsync(x => x.Type == model.LifestyleType);

            foreach (var allergenName in model.AllergenNames)
            {
                recipe.Allergens.Add(new RecipeAllergen
                {
                    Allergen = await this.allergenRepository.All().SingleOrDefaultAsync(x => x.Name == allergenName),
                });
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
