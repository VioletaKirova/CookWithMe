namespace CookWithMe.Services.Data.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Common;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.NutritionalValues;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class RecipeService : IRecipeService
    {
        private const string InvalidRecipeIdErrorMessage = "Recipe with ID: {0} does not exist.";
        private const string InvalidRecipePropertyErrorMessage = "One or more required properties are null.";
        private const string EmptyLifestylesCollectionErrorMessage = "Lifestyles collection is empty.";

        private readonly IDeletableEntityRepository<Recipe> recipeRepository;
        private readonly ICategoryService categoryService;
        private readonly ILifestyleService lifestyleService;
        private readonly IRecipeLifestyleService recipeLifestyleService;
        private readonly IUserService userService;
        private readonly IShoppingListService shoppingListService;
        private readonly IUserShoppingListService userShoppingListService;
        private readonly INutritionalValueService nutritionalValueService;
        private readonly IAllergenService allergenService;
        private readonly IRecipeAllergenService recipeAllergenService;
        private readonly IUserFavoriteRecipeService userFavoriteRecipeService;
        private readonly IUserCookedRecipeService userCookedRecipeService;
        private readonly IUserAllergenService userAllergenService;
        private readonly IStringFormatService stringFormatService;

        public RecipeService(
            IDeletableEntityRepository<Recipe> recipeRepository,
            ICategoryService categoryService,
            ILifestyleService lifestyleService,
            IRecipeLifestyleService recipeLifestyleService,
            IUserService userService,
            IShoppingListService shoppingListService,
            IUserShoppingListService userShoppingListService,
            INutritionalValueService nutritionalValueService,
            IAllergenService allergenService,
            IRecipeAllergenService recipeAllergenService,
            IUserFavoriteRecipeService userFavoriteRecipeService,
            IUserCookedRecipeService userCookedRecipeService,
            IUserAllergenService userAllergenService,
            IStringFormatService stringFormatService)
        {
            this.recipeRepository = recipeRepository;
            this.categoryService = categoryService;
            this.lifestyleService = lifestyleService;
            this.recipeLifestyleService = recipeLifestyleService;
            this.userService = userService;
            this.shoppingListService = shoppingListService;
            this.userShoppingListService = userShoppingListService;
            this.nutritionalValueService = nutritionalValueService;
            this.allergenService = allergenService;
            this.recipeAllergenService = recipeAllergenService;
            this.userFavoriteRecipeService = userFavoriteRecipeService;
            this.userCookedRecipeService = userCookedRecipeService;
            this.userAllergenService = userAllergenService;
            this.stringFormatService = stringFormatService;
        }

        public async Task<bool> CreateAsync(RecipeServiceModel recipeServiceModel)
        {
            if (recipeServiceModel.Title == null ||
                recipeServiceModel.Photo == null ||
                recipeServiceModel.Category == null ||
                recipeServiceModel.Summary == null ||
                recipeServiceModel.Directions == null ||
                recipeServiceModel.ShoppingList == null ||
                recipeServiceModel.NutritionalValue == null ||
                recipeServiceModel.UserId == null)
            {
                throw new ArgumentNullException(InvalidRecipePropertyErrorMessage);
            }

            if (recipeServiceModel.Lifestyles.Count == 0)
            {
                throw new EmptyCollectionException(EmptyLifestylesCollectionErrorMessage);
            }

            var recipe = recipeServiceModel.To<Recipe>();

            await this.categoryService.SetCategoryToRecipeAsync(recipeServiceModel.Category.Title, recipe);

            recipe.Allergens = new HashSet<RecipeAllergen>();
            recipe.Lifestyles = new HashSet<RecipeLifestyle>();

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipeAsync(recipeServiceModel.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeIdAsync(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeIdAsync(recipe.Id);

            foreach (var recipeAllergen in recipeServiceModel.Allergens)
            {
                await this.allergenService.SetAllergenToRecipeAsync(recipeAllergen.Allergen.Name, recipe);
            }

            foreach (var recipeLifestyle in recipeServiceModel.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipeAsync(recipeLifestyle.Lifestyle.Type, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IQueryable<RecipeServiceModel>> GetAllFilteredAsync(string userId)
        {
            var user = await this.userService.GetByIdAsync(userId);

            IQueryable<RecipeServiceModel> recipesFilteredByLifestyle = null;

            if (user.LifestyleId != null)
            {
                var userLifestyleId = user.LifestyleId;

                recipesFilteredByLifestyle = this.recipeRepository
                    .AllAsNoTracking()
                    .Where(x => x.Lifestyles.Select(r => r.Lifestyle.Id).Contains(userLifestyleId.Value))
                    .To<RecipeServiceModel>();
            }
            else
            {
                recipesFilteredByLifestyle = this.recipeRepository
                    .AllAsNoTracking()
                    .To<RecipeServiceModel>();
            }

            IQueryable<RecipeServiceModel> recipesFilteredByLifestyleAndAllergens = null;

            var userAllergenIds = (await this.userAllergenService.GetByUserIdAsync(userId))
                .Select(x => x.AllergenId);
            var recipeAllergenIds = await this.recipeAllergenService
                .GetRecipeIdsByAllergenIdsAsync(userAllergenIds);

            if (userAllergenIds.Count() > 0)
            {
                recipesFilteredByLifestyleAndAllergens = recipesFilteredByLifestyle
                    .Where(x => !recipeAllergenIds.Contains(x.Id));
            }

            return recipesFilteredByLifestyleAndAllergens == null ?
                recipesFilteredByLifestyle.OrderByDescending(x => x.CreatedOn) :
                recipesFilteredByLifestyleAndAllergens.OrderByDescending(x => x.CreatedOn);
        }

        public async Task<RecipeServiceModel> GetByIdAsync(string id)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            if (recipe == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, id));
            }

            var recipeServiceModel = recipe.To<RecipeServiceModel>();

            recipeServiceModel.User = await this.userService.GetByIdAsync(recipe.UserId);
            recipeServiceModel.Category = await this.categoryService.GetByIdAsync(recipe.CategoryId);
            recipeServiceModel.ShoppingList = await this.shoppingListService.GetByIdAsync(recipe.ShoppingListId);
            recipeServiceModel.NutritionalValue = await this.nutritionalValueService.GetByIdAsync(recipe.NutritionalValueId);
            recipeServiceModel.Allergens = await this.recipeAllergenService.GetByRecipeIdAsync(id);
            recipeServiceModel.Lifestyles = await this.recipeLifestyleService.GetByRecipeIdAsync(id);

            return recipeServiceModel;
        }

        public async Task SetRecipeToReviewAsync(string recipeId, Review review)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            if (recipe == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, recipeId));
            }

            review.Recipe = recipe;
        }

        public async Task<bool> SetRecipeToUserFavoriteRecipesAsync(string userId, string recipeId)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            if (recipe == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, recipeId));
            }

            var result = await this.userService.SetFavoriteRecipeAsync(userId, recipe);

            return result;
        }

        public async Task<bool> SetRecipeToUserCookedRecipesAsync(string userId, string recipeId)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(recipeId);

            if (recipe == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, recipeId));
            }

            var result = await this.userService.SetCookedRecipeAsync(userId, recipe);

            return result;
        }

        public async Task<bool> EditAsync(string id, RecipeServiceModel recipeServiceModel)
        {
            var recipeFromDb = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            if (recipeFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, id));
            }

            if (recipeServiceModel.Title == null ||
                recipeServiceModel.Photo == null ||
                recipeServiceModel.Category == null ||
                recipeServiceModel.Summary == null ||
                recipeServiceModel.Directions == null ||
                recipeServiceModel.ShoppingList == null ||
                recipeServiceModel.NutritionalValue == null ||
                recipeServiceModel.UserId == null)
            {
                throw new ArgumentNullException(InvalidRecipePropertyErrorMessage);
            }

            if (recipeServiceModel.Lifestyles.Count == 0)
            {
                throw new EmptyCollectionException(EmptyLifestylesCollectionErrorMessage);
            }

            recipeFromDb.Title = recipeServiceModel.Title;
            recipeFromDb.Summary = recipeServiceModel.Summary;
            recipeFromDb.Directions = recipeServiceModel.Directions;
            recipeFromDb.PreparationTime = recipeServiceModel.PreparationTime;
            recipeFromDb.CookingTime = recipeServiceModel.CookingTime;
            recipeFromDb.NeededTime = recipeServiceModel.NeededTime;
            recipeFromDb.SkillLevel = recipeServiceModel.SkillLevel;
            recipeFromDb.Serving = recipeServiceModel.Serving;
            recipeFromDb.Yield = recipeServiceModel.Yield;

            await this.categoryService.SetCategoryToRecipeAsync(recipeServiceModel.Category.Title, recipeFromDb);

            await this.shoppingListService.EditAsync(recipeFromDb.ShoppingListId, recipeServiceModel.ShoppingList);
            await this.nutritionalValueService.EditAsync(recipeFromDb.NutritionalValueId, recipeServiceModel.NutritionalValue);

            this.recipeAllergenService.DeletePreviousRecipeAllergensByRecipeId(recipeFromDb.Id);
            this.recipeLifestyleService.DeletePreviousRecipeLifestylesByRecipeId(recipeFromDb.Id);

            foreach (var recipeAllergen in recipeServiceModel.Allergens)
            {
                await this.allergenService.SetAllergenToRecipeAsync(recipeAllergen.Allergen.Name, recipeFromDb);
            }

            foreach (var recipeLifestyle in recipeServiceModel.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipeAsync(recipeLifestyle.Lifestyle.Type, recipeFromDb);
            }

            this.recipeRepository.Update(recipeFromDb);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var recipeFromDb = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            if (recipeFromDb == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidRecipeIdErrorMessage, id));
            }

            await this.userShoppingListService.DeleteByShoppingListIdAsync(recipeFromDb.ShoppingListId);
            await this.userFavoriteRecipeService.DeleteByRecipeIdAsync(id);
            await this.userCookedRecipeService.DeleteByRecipeIdAsync(id);

            await this.shoppingListService.DeleteByIdAsync(recipeFromDb.ShoppingListId);
            await this.nutritionalValueService.DeleteByIdAsync(recipeFromDb.NutritionalValueId);

            this.recipeRepository.Delete(recipeFromDb);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<RecipeServiceModel> GetByCategoryId(int categoryId)
        {
            return this.recipeRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>();
        }

        public IQueryable<RecipeServiceModel> GetByUserId(string userId)
        {
            return this.recipeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>();
        }

        public async Task<IQueryable<RecipeServiceModel>> GetBySearchValuesAsync(RecipeBrowseServiceModel recipeBrowseServiceModel)
        {
            var filteredRecipes = this.recipeRepository.AllAsNoTracking();

            if (!string.IsNullOrEmpty(recipeBrowseServiceModel.KeyWords) &&
                !string.IsNullOrWhiteSpace(recipeBrowseServiceModel.KeyWords))
            {
                var keyWords = this.stringFormatService
                    .SplitByCommaAndWhitespace(recipeBrowseServiceModel.KeyWords.ToLower());

                filteredRecipes = filteredRecipes.Where(x =>
                    keyWords.Any(kw => x.Title.ToLower().Contains(kw)));
            }

            if (recipeBrowseServiceModel.Category.Title != null)
            {
                var categoryId = await this.categoryService.GetIdByTitleAsync(recipeBrowseServiceModel.Category.Title);
                filteredRecipes = filteredRecipes.Where(x => x.CategoryId == categoryId);
            }

            if (recipeBrowseServiceModel.Lifestyle.Type != null)
            {
                var lifestyleId = await this.lifestyleService.GetIdByTypeAsync(recipeBrowseServiceModel.Lifestyle.Type);
                var recipeLifestyleIds = await this.recipeLifestyleService.GetRecipeIdsByLifestyleIdAsync(lifestyleId);
                filteredRecipes = filteredRecipes.Where(x => recipeLifestyleIds.Contains(x.Id));
            }

            if (recipeBrowseServiceModel.Allergens.Any())
            {
                var allergenIds = await this.allergenService.GetIdsByNamesAsync(recipeBrowseServiceModel.Allergens.Select(x => x.Allergen.Name));
                var recipeAllergenIds = await this.recipeAllergenService.GetRecipeIdsByAllergenIdsAsync(allergenIds);
                filteredRecipes = filteredRecipes.Where(x => !recipeAllergenIds.Contains(x.Id));
            }

            if (recipeBrowseServiceModel.SkillLevel != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.SkillLevel == recipeBrowseServiceModel.SkillLevel);
            }

            if (recipeBrowseServiceModel.Serving != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.Serving == recipeBrowseServiceModel.Serving);
            }

            if (recipeBrowseServiceModel.NeededTime != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.NeededTime == recipeBrowseServiceModel.NeededTime);
            }

            if (recipeBrowseServiceModel.NutritionalValue != null)
            {
                if (recipeBrowseServiceModel.NutritionalValue.Calories != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Calories == recipeBrowseServiceModel.NutritionalValue.Calories);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Fats != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Fats == recipeBrowseServiceModel.NutritionalValue.Fats);
                }

                if (recipeBrowseServiceModel.NutritionalValue.SaturatedFats != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.SaturatedFats == recipeBrowseServiceModel.NutritionalValue.SaturatedFats);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Carbohydrates != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Carbohydrates == recipeBrowseServiceModel.NutritionalValue.Carbohydrates);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Sugar != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Sugar == recipeBrowseServiceModel.NutritionalValue.Sugar);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Protein != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Protein == recipeBrowseServiceModel.NutritionalValue.Protein);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Fiber != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Fiber == recipeBrowseServiceModel.NutritionalValue.Fiber);
                }

                if (recipeBrowseServiceModel.NutritionalValue.Salt != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Salt == recipeBrowseServiceModel.NutritionalValue.Salt);
                }
            }

            if (recipeBrowseServiceModel.Yield != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.Yield == recipeBrowseServiceModel.Yield);
            }

            return filteredRecipes
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>();
        }
    }
}
