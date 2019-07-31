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

            recipe.Allergens = new HashSet<RecipeAllergen>();

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipe(model.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            foreach (var recipeAllergen in model.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<RecipeServiceModel>> GetAllFiltered(string userId)
        {
            var user = await this.userService.GetById(userId);

            var userLifestyleId = user.LifestyleId;
            var userAllergyNames = user.Allergies.Select(x => x.Allergen.Name);

            var recipesFilteredByLifestyle = await this.recipeRepository
                .AllAsNoTracking()
                .To<RecipeServiceModel>()
                .Where(x => x.LifestyleId == userLifestyleId)
                .ToListAsync();

            List<RecipeServiceModel> recipesFilteredByLifestyleAndAllergies = new List<RecipeServiceModel>();

            var userAllergyNamesJoined = string.Join(" ", userAllergyNames);

            foreach (var recipe in recipesFilteredByLifestyle)
            {
                if (recipe.Allergens.Select(x => x.Allergen.Name).Any(x => userAllergyNamesJoined.Contains(x)))
                {
                    continue;
                }

                recipesFilteredByLifestyleAndAllergies.Add(recipe);
            }

            return recipesFilteredByLifestyleAndAllergies?.OrderByDescending(x => x.CreatedOn);
        }
    }
}
