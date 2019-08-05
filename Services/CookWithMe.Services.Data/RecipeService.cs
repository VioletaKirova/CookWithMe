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
        private readonly IRecipeLifestyleService recipeLifestyleService;
        private readonly IUserService userService;
        private readonly IShoppingListService shoppingListService;
        private readonly INutritionalValueService nutritionalValueService;
        private readonly IAllergenService allergenService;
        private readonly IRecipeAllergenService recipeAllergenService;

        public RecipeService(
            IDeletableEntityRepository<Recipe> recipeRepository,
            ICategoryService categoryService,
            ILifestyleService lifestyleService,
            IRecipeLifestyleService recipeLifestyleService,
            IUserService userService,
            IShoppingListService shoppingListService,
            INutritionalValueService nutritionalValueService,
            IAllergenService allergenService,
            IRecipeAllergenService recipeAllergenService)
        {
            this.recipeRepository = recipeRepository;
            this.categoryService = categoryService;
            this.lifestyleService = lifestyleService;
            this.recipeLifestyleService = recipeLifestyleService;
            this.userService = userService;
            this.shoppingListService = shoppingListService;
            this.nutritionalValueService = nutritionalValueService;
            this.allergenService = allergenService;
            this.recipeAllergenService = recipeAllergenService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel recipeServiceModel)
        {
            var recipe = recipeServiceModel.To<Recipe>();

            await this.categoryService.SetCategoryToRecipe(recipeServiceModel.Category.Title, recipe);

            recipe.Allergens = new HashSet<RecipeAllergen>();
            recipe.Lifestyles = new HashSet<RecipeLifestyle>();

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipe(recipeServiceModel.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            foreach (var recipeAllergen in recipeServiceModel.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipe);
            }

            foreach (var recipeLifestyle in recipeServiceModel.Lifestyles)
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
            recipeServiceModel.Category = await this.categoryService.GetById(recipe.CategoryId);
            recipeServiceModel.ShoppingList = await this.shoppingListService.GetById(recipe.ShoppingListId);
            recipeServiceModel.NutritionalValue = await this.nutritionalValueService.GetById(recipe.NutritionalValueId);
            recipeServiceModel.Allergens = await this.recipeAllergenService.GetByRecipeId(recipe.Id);
            recipeServiceModel.Lifestyles = await this.recipeLifestyleService.GetByRecipeId(recipe.Id);

            return recipeServiceModel;
        }

        public async Task SetRecipeToReview(string recipeId, Review review)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            review.Recipe = recipe;
        }

        public async Task<bool> SetRecipeToUserFavoriteRecipes(string userId, string recipeId)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            var result = await this.userService.SetFavoriteRecipe(userId, recipe);

            return result;
        }

        public async Task<bool> SetRecipeToUserCookedRecipes(string userId, string recipeId)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            var result = await this.userService.SetCookedRecipe(userId, recipe);

            return result;
        }

        public async Task<bool> Edit(string id, RecipeServiceModel model)
        {
            var recipeFromDb = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            recipeFromDb.Title = model.Title;
            recipeFromDb.Summary = model.Summary;
            recipeFromDb.Directions = model.Directions;
            recipeFromDb.PreparationTime = model.PreparationTime;
            recipeFromDb.CookingTime = model.CookingTime;
            recipeFromDb.NeededTime = model.NeededTime;
            recipeFromDb.SkillLevel = model.SkillLevel;
            recipeFromDb.Serving = model.Serving;
            recipeFromDb.Yield = model.Yield;

            await this.categoryService.SetCategoryToRecipe(model.Category.Title, recipeFromDb);

            await this.shoppingListService.Edit(recipeFromDb.ShoppingListId, model.ShoppingList);
            await this.nutritionalValueService.Edit(recipeFromDb.NutritionalValueId, model.NutritionalValue);

            this.recipeAllergenService.DeletePreviousAllergensByRecipeId(recipeFromDb.Id);
            this.recipeLifestyleService.DeletePreviousLifestylesByRecipeId(recipeFromDb.Id);

            foreach (var recipeAllergen in model.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipeFromDb);
            }

            foreach (var recipeLifestyle in model.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipe(recipeLifestyle.Lifestyle.Type, recipeFromDb);
            }

            this.recipeRepository.Update(recipeFromDb);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
