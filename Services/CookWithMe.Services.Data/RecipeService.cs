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

            recipe.Allergens = new HashSet<RecipeAllergen>();
            recipe.Lifestyles = new HashSet<RecipeLifestyle>();

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipe(model.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            foreach (var recipeAllergen in model.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipe);
            }

            foreach (var recipeLifestyle in model.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipe(recipeLifestyle.Lifestyle.Type, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<RecipeServiceModel>> GetAllFiltered(string userId)
        {
            var user = await this.userService.GetById(userId);

            var recipesFilteredByLifestyle = new List<RecipeServiceModel>();

            if (user.LifestyleId != null)
            {
                var userLifestyleId = user.LifestyleId;

                recipesFilteredByLifestyle = await this.recipeRepository
                    .AllAsNoTracking()
                    .Where(x => x.Lifestyles.Select(r => r.Lifestyle.Id).Contains(userLifestyleId.Value))
                    .To<RecipeServiceModel>()
                    .ToListAsync();
            }
            else
            {
                recipesFilteredByLifestyle = await this.recipeRepository
                    .AllAsNoTracking()
                    .To<RecipeServiceModel>()
                    .ToListAsync();
            }

            var recipesFilteredByLifestyleAndAllergies = new List<RecipeServiceModel>();

            if (user.Allergies != null)
            {
                var userAllergens = user.Allergies.Select(x => x.Allergen);

                foreach (var recipe in recipesFilteredByLifestyle)
                {
                    var recipeAllergens = recipe.Allergens.Select(x => x.Allergen);

                    if (recipeAllergens.Any(x => userAllergens.Contains(x)))
                    {
                        continue;
                    }

                    recipesFilteredByLifestyleAndAllergies.Add(recipe);
                }
            }

            return recipesFilteredByLifestyleAndAllergies.Count() == 0 ?
                recipesFilteredByLifestyle.OrderByDescending(x => x.CreatedOn) :
                recipesFilteredByLifestyleAndAllergies.OrderByDescending(x => x.CreatedOn);
        }

        public async Task<RecipeServiceModel> GetById(string id)
        {
            var recipe = await this.recipeRepository.All().SingleOrDefaultAsync(x => x.Id == id);
            var recipeServiceModel = recipe.To<RecipeServiceModel>();

            recipeServiceModel.User = await this.userService.GetById(recipe.UserId);
            recipeServiceModel.ShoppingList = await this.shoppingListService.GetById(recipe.ShoppingListId);
            recipeServiceModel.NutritionalValue = await this.nutritionalValueService.GetById(recipe.NutritionalValueId);

            return recipeServiceModel;
        }
    }
}
