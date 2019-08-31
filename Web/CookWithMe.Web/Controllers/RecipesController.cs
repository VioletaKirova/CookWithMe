namespace CookWithMe.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    using CookWithMe.Web.Filters;
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
        private const string AddToFavoriteErrorMessage = "Failed to add the recipe to Favorite Recipes.";
        private const string RemoveFromFavoriteErrorMessage = "Failed to remove the recipe from Favorite Recipes.";
        private const string AddToCookedErrorMessage = "Failed to add the recipe to Cooked Recipes.";
        private const string RemoveFromCookedErrorMessage = "Failed to remove the recipe from Cooked Recipes.";

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
        [ServiceFilter(typeof(ArgumentNullExceptionFilterAttribute))]
        public async Task<IActionResult> Details(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);

            recipeServiceModel.Reviews = await this.reviewService.GetByRecipeId(id).ToListAsync();

            var recipeDetailsViewModel = recipeServiceModel.To<RecipeDetailsViewModel>();

            recipeDetailsViewModel.DirectionsList = this.stringFormatService
                .SplitBySemicollon(recipeServiceModel.Directions);

            recipeDetailsViewModel.ShoppingListIngredients = this.stringFormatService
                .SplitBySemicollon(recipeServiceModel.ShoppingList.Ingredients);

            recipeDetailsViewModel.Allergens = recipeServiceModel.Allergens
                .Select(x => x.Allergen.Name)
                .ToList();

            recipeDetailsViewModel.Lifestyles = recipeServiceModel.Lifestyles
                .Select(x => x.Lifestyle.Type)
                .ToList();

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
        public async Task<IActionResult> AddToFavorite(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.recipeService.SetRecipeToUserFavoriteRecipesAsync(userId, id))
            {
                this.TempData["Error"] = AddToFavoriteErrorMessage;
            }

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromFavorite(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.userFavoriteRecipeService.DeleteByUserIdAndRecipeIdAsync(userId, id))
            {
                this.TempData["Error"] = RemoveFromFavoriteErrorMessage;
            }

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddToCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.recipeService.SetRecipeToUserCookedRecipesAsync(userId, id))
            {
                this.TempData["Error"] = AddToCookedErrorMessage;
            }

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!await this.userCookedRecipeService.DeleteByUserIdAndRecipeIdAsync(userId, id))
            {
                this.TempData["Error"] = RemoveFromCookedErrorMessage;
            }

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Favorite(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriteRecipes = this.userFavoriteRecipeService
                .GetRecipesByUserId(userId)
                .To<RecipeFavoriteViewModel>();

            return this.View(await PaginatedList<RecipeFavoriteViewModel>
                .CreateAsync(favoriteRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cooked(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cookedRecipes = this.userCookedRecipeService
                .GetRecipesByUserId(userId)
                .To<RecipeCookedViewModel>();

            return this.View(await PaginatedList<RecipeCookedViewModel>
                .CreateAsync(cookedRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            var recipeBrowseInputModel = new RecipeBrowseInputModel()
            {
                RecipeViewData = await this.GetRecipeViewDataModelAsync(),
            };

            return this.View(recipeBrowseInputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Filtered(RecipeBrowseInputModel recipeBrowseInputModel, int? pageNumber)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Browse));
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

            return this.View(await PaginatedList<RecipeBrowseViewModel>
                .CreateAsync(filteredRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        // TODO: Move to service
        private async Task<RecipeViewDataModel> GetRecipeViewDataModelAsync()
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
