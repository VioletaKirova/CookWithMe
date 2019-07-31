namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using Microsoft.EntityFrameworkCore;

    public class RecipeService : IRecipeService
    {
        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly ICategoryService categoryService;
        private readonly ILifestyleService lifestyleService;
        private readonly IUserService userService;
        private readonly IShoppingListService shoppingListService;
        private readonly INutritionalValueService nutritionalValueService;
        private readonly IAllergenService allergenService;

        public RecipeService(
            IDeletableEntityRepository<Recipe> recipeRepository,
            ICategoryService categoryService,
            ILifestyleService lifestyleService,
            IUserService userService,
            IShoppingListService shoppingListService,
            INutritionalValueService nutritionalValueService,
            IAllergenService allergenService)
        {
            this.recipeRepository = recipeRepository;
            this.categoryService = categoryService;
            this.lifestyleService = lifestyleService;
            this.userService = userService;
            this.shoppingListService = shoppingListService;
            this.nutritionalValueService = nutritionalValueService;
            this.allergenService = allergenService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel model)
        {
            var recipe = AutoMapper.Mapper.Map<RecipeServiceModel, Recipe>(model);

            await this.categoryService.SetCategoryToRecipe(model.Category.Title, recipe);
            await this.lifestyleService.SetLifestyleToRecipe(model.Lifestyle.Type, recipe);

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipe(model.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            foreach (var allergenName in model.AllergenNames)
            {
                await this.allergenService.SetAllergenToRecipe(allergenName, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<RecipeServiceModel>> GetAllFiltered(string userId)
        {
            var user = await this.userService.GetById(userId);

            var userLifestyle = user.Lifestyle;
            var userAllergyNames = user.Allergies.Select(x => x.Allergen.Name);

            var recipesFilteredByLifestyle = await this.recipeRepository
                .AllAsNoTracking()
                .To<RecipeServiceModel>()
                .Where(x => x.Lifestyle == userLifestyle)
                .ToListAsync();

            List<RecipeServiceModel> recipesFilteredByLifestyleAndAllergies = null;

            var userAllergyNamesJoined = string.Join(" ", userAllergyNames);

            foreach (var recipe in recipesFilteredByLifestyle)
            {
                if (recipe.AllergenNames.Any(x => userAllergyNamesJoined.Contains(x)))
                {
                    continue;
                }

                recipesFilteredByLifestyleAndAllergies.Add(recipe);
            }

            return recipesFilteredByLifestyleAndAllergies;
        }
    }
}
