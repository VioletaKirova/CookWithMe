namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeService : IRecipeService
    {
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

        public async Task<bool> CreateAsync(RecipeServiceModel serviceModel)
        {
            var recipe = serviceModel.To<Recipe>();

            await this.categoryService.SetCategoryToRecipe(serviceModel.Category.Title, recipe);

            recipe.Allergens = new HashSet<RecipeAllergen>();
            recipe.Lifestyles = new HashSet<RecipeLifestyle>();

            await this.recipeRepository.AddAsync(recipe);
            await this.recipeRepository.SaveChangesAsync();

            await this.userService.SetUserToRecipe(serviceModel.UserId, recipe);

            recipe.ShoppingListId = await this.shoppingListService.GetIdByRecipeId(recipe.Id);
            recipe.NutritionalValueId = await this.nutritionalValueService.GetIdByRecipeId(recipe.Id);

            foreach (var recipeAllergen in serviceModel.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipe);
            }

            foreach (var recipeLifestyle in serviceModel.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipe(recipeLifestyle.Lifestyle.Type, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IQueryable<RecipeServiceModel>> GetAllFiltered(string userId)
        {
            var user = await this.userService.GetById(userId);

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

            var userAllergenIds = (await this.userAllergenService.GetByUserId(userId))
                .Select(x => x.AllergenId);
            var recipeAllergenIds = await this.recipeAllergenService
                .GetAllRecipeIdsByAllergenIds(userAllergenIds);

            if (userAllergenIds.Count() > 0)
            {
                recipesFilteredByLifestyleAndAllergens = recipesFilteredByLifestyle
                    .Where(x => !recipeAllergenIds.Contains(x.Id));
            }

            return recipesFilteredByLifestyleAndAllergens == null ?
                recipesFilteredByLifestyle.OrderByDescending(x => x.CreatedOn) :
                recipesFilteredByLifestyleAndAllergens.OrderByDescending(x => x.CreatedOn);
        }

        public async Task<RecipeServiceModel> GetById(string id)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(id);
            var recipeServiceModel = recipe.To<RecipeServiceModel>();

            recipeServiceModel.User = await this.userService.GetById(recipe.UserId);
            recipeServiceModel.Category = await this.categoryService.GetById(recipe.CategoryId);
            recipeServiceModel.ShoppingList = await this.shoppingListService.GetById(recipe.ShoppingListId);
            recipeServiceModel.NutritionalValue = await this.nutritionalValueService.GetById(recipe.NutritionalValueId);
            recipeServiceModel.Allergens = await this.recipeAllergenService.GetByRecipeId(id);
            recipeServiceModel.Lifestyles = await this.recipeLifestyleService.GetByRecipeId(id);

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

        public async Task<bool> Edit(string id, RecipeServiceModel serviceModel)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            recipe.Title = serviceModel.Title;
            recipe.Summary = serviceModel.Summary;
            recipe.Directions = serviceModel.Directions;
            recipe.PreparationTime = serviceModel.PreparationTime;
            recipe.CookingTime = serviceModel.CookingTime;
            recipe.NeededTime = serviceModel.NeededTime;
            recipe.SkillLevel = serviceModel.SkillLevel;
            recipe.Serving = serviceModel.Serving;
            recipe.Yield = serviceModel.Yield;

            await this.categoryService.SetCategoryToRecipe(serviceModel.Category.Title, recipe);

            await this.shoppingListService.Edit(recipe.ShoppingListId, serviceModel.ShoppingList);
            await this.nutritionalValueService.Edit(recipe.NutritionalValueId, serviceModel.NutritionalValue);

            this.recipeAllergenService.DeletePreviousRecipeAllergensByRecipeId(recipe.Id);
            this.recipeLifestyleService.DeletePreviousRecipeLifestylesByRecipeId(recipe.Id);

            foreach (var recipeAllergen in serviceModel.Allergens)
            {
                await this.allergenService.SetAllergenToRecipe(recipeAllergen.Allergen.Name, recipe);
            }

            foreach (var recipeLifestyle in serviceModel.Lifestyles)
            {
                await this.lifestyleService.SetLifestyleToRecipe(recipeLifestyle.Lifestyle.Type, recipe);
            }

            this.recipeRepository.Update(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> Delete(string id)
        {
            var recipe = await this.recipeRepository.GetByIdWithDeletedAsync(id);

            await this.userShoppingListService.DeleteByShoppingListId(recipe.ShoppingListId);
            await this.userFavoriteRecipeService.DeleteByRecipeId(id);
            await this.userCookedRecipeService.DeleteByRecipeId(id);

            await this.shoppingListService.Delete(recipe.ShoppingListId);
            await this.nutritionalValueService.Delete(recipe.NutritionalValueId);

            this.recipeRepository.Delete(recipe);
            var result = await this.recipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<RecipeServiceModel> GetAllByCategoryId(int categoryId)
        {
            return this.recipeRepository
                .AllAsNoTracking()
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>();
        }

        public IQueryable<RecipeServiceModel> GetByIds(IEnumerable<string> recipeIds)
        {
            return this.recipeRepository
                .AllAsNoTracking()
                .Where(x => recipeIds.Contains(x.Id))
                .To<RecipeServiceModel>();
        }

        public IQueryable<RecipeServiceModel> GetAllByUserId(string userId)
        {
            return this.recipeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>();
        }

        public async Task<IQueryable<RecipeServiceModel>> GetAllBySearch(RecipeSearchServiceModel serviceModel)
        {
            var filteredRecipes = this.recipeRepository.AllAsNoTracking();

            if (serviceModel.KeyWords != null)
            {
                var keyWords = this.stringFormatService
                    .SplitByCommaAndWhitespace(serviceModel.KeyWords.ToLower());

                filteredRecipes = filteredRecipes.Where(x =>
                    keyWords.Any(kw => x.Title.ToLower().Contains(kw)));
            }

            if (serviceModel.Category.Title != null)
            {
                var categoryId = await this.categoryService.GetId(serviceModel.Category.Title);
                filteredRecipes = filteredRecipes.Where(x => x.CategoryId == categoryId);
            }

            if (serviceModel.Lifestyle.Type != null)
            {
                var lifestyleId = await this.lifestyleService.GetId(serviceModel.Lifestyle.Type);
                var recipeLifestyleIds = await this.recipeLifestyleService.GetAllRecipeIdsByLifestyleId(lifestyleId);
                filteredRecipes = filteredRecipes.Where(x => recipeLifestyleIds.Contains(x.Id));
            }

            if (serviceModel.Allergens.Any())
            {
                var allergenIds = await this.allergenService.GetAllIds(serviceModel.Allergens.Select(x => x.Allergen.Name));
                var recipeAllergenIds = await this.recipeAllergenService.GetAllRecipeIdsByAllergenIds(allergenIds);
                filteredRecipes = filteredRecipes.Where(x => !recipeAllergenIds.Contains(x.Id));
            }

            if (serviceModel.SkillLevel != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.SkillLevel == serviceModel.SkillLevel);
            }

            if (serviceModel.Serving != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.Serving == serviceModel.Serving);
            }

            if (serviceModel.NeededTime != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.NeededTime == serviceModel.NeededTime);
            }

            if (serviceModel.NutritionalValue != null)
            {
                if (serviceModel.NutritionalValue.Calories != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Calories == serviceModel.NutritionalValue.Calories);
                }

                if (serviceModel.NutritionalValue.Fats != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Fats == serviceModel.NutritionalValue.Fats);
                }

                if (serviceModel.NutritionalValue.SaturatedFats != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.SaturatedFats == serviceModel.NutritionalValue.SaturatedFats);
                }

                if (serviceModel.NutritionalValue.Carbohydrates != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Carbohydrates == serviceModel.NutritionalValue.Carbohydrates);
                }

                if (serviceModel.NutritionalValue.Sugar != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Sugar == serviceModel.NutritionalValue.Sugar);
                }

                if (serviceModel.NutritionalValue.Protein != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Protein == serviceModel.NutritionalValue.Protein);
                }

                if (serviceModel.NutritionalValue.Fiber != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Fiber == serviceModel.NutritionalValue.Fiber);
                }

                if (serviceModel.NutritionalValue.Salt != null)
                {
                    filteredRecipes = filteredRecipes.Where(x => x.NutritionalValue.Salt == serviceModel.NutritionalValue.Salt);
                }
            }

            if (serviceModel.Yield != null)
            {
                filteredRecipes = filteredRecipes.Where(x => x.Yield == serviceModel.Yield);
            }

            return filteredRecipes.To<RecipeServiceModel>();
        }
    }
}
