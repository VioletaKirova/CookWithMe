namespace CookWithMe.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Reviews;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.InputModels.Recipes.Browse;
    using CookWithMe.Web.ViewModels.Recipes.Browse;
    using CookWithMe.Web.ViewModels.Recipes.Cooked;
    using CookWithMe.Web.ViewModels.Recipes.Details;
    using CookWithMe.Web.ViewModels.Recipes.Favorite;
    using CookWithMe.Web.ViewModels.Recipes.ViewData;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserFavoriteRecipeService userFavoriteRecipeService;
        private readonly IUserCookedRecipeService userCookedRecipeService;
        private readonly IReviewService reviewService;
        private readonly IStringFormatService stringFormatService;
        private readonly ICategoryService categoryService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly IEnumParseService enumParseService;

        public RecipesController(
            IRecipeService recipeService,
            IUserFavoriteRecipeService userFavoriteRecipeService,
            IUserCookedRecipeService userCookedRecipeService,
            IReviewService reviewService,
            IStringFormatService stringFormatService,
            ICategoryService categoryService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            IEnumParseService enumParseService)
        {
            this.recipeService = recipeService;
            this.userFavoriteRecipeService = userFavoriteRecipeService;
            this.userCookedRecipeService = userCookedRecipeService;
            this.reviewService = reviewService;
            this.stringFormatService = stringFormatService;
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.enumParseService = enumParseService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);
            recipeServiceModel.Reviews = await this.reviewService.GetByRecipeId(id).ToListAsync();

            var recipeDetailsViewModel = recipeServiceModel.To<RecipeDetailsViewModel>();

            recipeDetailsViewModel.DirectionsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.Directions);

            recipeDetailsViewModel.ShoppingListIngredients = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.ShoppingList.Ingredients);

            recipeDetailsViewModel.FormatedPreparationTime = this.stringFormatService
                .FormatTime(recipeServiceModel.PreparationTime);

            recipeDetailsViewModel.FormatedCookingTime = this.stringFormatService
                .FormatTime(recipeServiceModel.CookingTime);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            recipeDetailsViewModel.UserFavoritedCurrentRecipe = await this.userFavoriteRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, id);
            recipeDetailsViewModel.UserCookedCurrentRecipe = await this.userCookedRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, id);

            return this.View(recipeDetailsViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserFavoriteRecipesAsync(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userFavoriteRecipeService.DeleteByUserIdAndRecipeIdAsync(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddToCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserCookedRecipesAsync(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userCookedRecipeService.DeleteByUserIdAndRecipeIdAsync(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Favorite(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriteRecipeIds = await this.userFavoriteRecipeService
                .GetRecipeIdsByUserId(userId)
                .ToListAsync();

            var favoriteRecipes = this.recipeService
                .GetByIds(favoriteRecipeIds)
                .To<RecipeFavoriteViewModel>();

            return this.View(await PaginatedList<RecipeFavoriteViewModel>
                .CreateAsync(favoriteRecipes, pageNumber ?? 1, GlobalConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cooked(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cookedRecipeIds = await this.userCookedRecipeService
                .GetRecipeIdsByUserId(userId)
                .ToListAsync();

            var cookedRecipes = this.recipeService
                .GetByIds(cookedRecipeIds)
                .To<RecipeCookedViewModel>();

            return this.View(await PaginatedList<RecipeCookedViewModel>
                .CreateAsync(cookedRecipes, pageNumber ?? 1, GlobalConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Filtered(RecipeBrowseInputModel recipeBrowseInputModel, int? pageNumber)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: Refactor this
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                return this.View();
            }

            var recipeBrowseServiceModel = recipeBrowseInputModel.To<RecipeBrowseServiceModel>();

            if (recipeBrowseInputModel.NeededTime != null)
            {
                recipeBrowseServiceModel.NeededTime = this.enumParseService
                    .Parse<Period>(recipeBrowseInputModel.NeededTime);
            }

            foreach (var allergenName in recipeBrowseInputModel.AllergenNames)
            {
                recipeBrowseServiceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            var filteredRecipes = (await this.recipeService
                .GetBySearchValuesAsync(recipeBrowseServiceModel))
                .To<RecipeBrowseViewModel>();

            int pageSize = GlobalConstants.PageSize;
            return this.View(await PaginatedList<RecipeBrowseViewModel>.CreateAsync(filteredRecipes, pageNumber ?? 1, pageSize));
        }

        private async Task<RecipeViewDataModel> GetRecipeViewDataModel()
        {
            var categoryTitles = await this.categoryService.GetAllTitlesAsync();
            var allergenNames = await this.allergenService.GetAllNamesAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypesAsync();
            var periodNames = Enum.GetNames(typeof(Period));
            var levelNames = Enum.GetNames(typeof(Level));
            var sizeNames = Enum.GetNames(typeof(Size));

            var periodDescriptions = new List<string>();
            foreach (var periodName in periodNames)
            {
                periodDescriptions.Add(this.enumParseService
                    .GetEnumDescription(periodName, typeof(Period)));
            }

            var recipeViewDataModel = new RecipeViewDataModel
            {
                CategoryTitles = categoryTitles,
                AllergenNames = allergenNames,
                LifestyleTypes = lifestyleTypes,
                PeriodValues = periodDescriptions,
                LevelValues = levelNames,
                SizeValues = sizeNames,
            };

            return recipeViewDataModel;
        }
    }
}
