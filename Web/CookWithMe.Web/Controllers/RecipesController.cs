namespace CookWithMe.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Recipes;
    using CookWithMe.Web.ViewModels.Recipes;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class RecipesController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly IUserFavoriteRecipeService userFavoriteRecipeService;
        private readonly IUserCookedRecipeService userCookedRecipeService;
        private readonly IReviewService reviewService;
        private readonly IStringFormatService stringFormatService;
        private readonly ICategoryService categoryService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;

        public RecipesController(
            IRecipeService recipeService,
            IUserService userService,
            IUserFavoriteRecipeService userFavoriteRecipeService,
            IUserCookedRecipeService userCookedRecipeService,
            IReviewService reviewService,
            IStringFormatService stringFormatService,
            ICategoryService categoryService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.userFavoriteRecipeService = userFavoriteRecipeService;
            this.userCookedRecipeService = userCookedRecipeService;
            this.reviewService = reviewService;
            this.stringFormatService = stringFormatService;
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var recipeServiceModel = await this.recipeService.GetById(id);
            recipeServiceModel.Reviews = await this.reviewService.GetAllByRecipeId(id).ToListAsync();

            var recipeViewModel = recipeServiceModel.To<RecipeDetailsViewModel>();

            recipeViewModel.DirectionsList = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.Directions);

            recipeViewModel.ShoppingListIngredients = this.stringFormatService
                .SplitBySemicollonAndWhitespace(recipeServiceModel.ShoppingList.Ingredients);

            recipeViewModel.FormatedPreparationTime = this.stringFormatService
                .DisplayTime(recipeServiceModel.PreparationTime);

            recipeViewModel.FormatedCookingTime = this.stringFormatService
                .DisplayTime(recipeServiceModel.CookingTime);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            recipeViewModel.UserFavoritedCurrentRecipe = await this.userFavoriteRecipeService
                .ContainsByUserIdAndRecipeId(userId, id);
            recipeViewModel.UserCookedCurrentRecipe = await this.userCookedRecipeService
                .ContainsByUserIdAndRecipeId(userId, id);

            return this.View(recipeViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddToFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserFavoriteRecipes(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromFavorites(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userFavoriteRecipeService.DeleteByUserIdAndRecipeId(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddToCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.recipeService.SetRecipeToUserCookedRecipes(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveFromCooked(string id)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            await this.userCookedRecipeService.DeleteByUserIdAndRecipeId(userId, id);

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Favorite()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favoriteRecipeIds = await this.userFavoriteRecipeService
                .GetRecipeIdsByUserId(userId)
                .ToListAsync();

            var recipeFavoriteViewModel = await this.recipeService
                .GetByIds(favoriteRecipeIds)
                .To<RecipeFavoriteViewModel>()
                .ToListAsync();

            return this.View(recipeFavoriteViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Cooked()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cookedRecipeIds = await this.userCookedRecipeService
                .GetRecipeIdsByUserId(userId)
                .ToListAsync();

            var recipeCookedViewModel = await this.recipeService
                .GetByIds(cookedRecipeIds)
                .To<RecipeCookedViewModel>()
                .ToListAsync();

            return this.View(recipeCookedViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Search()
        {
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Filtered(RecipeSearchInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                return this.View();
            }

            var serviceModel = inputModel.To<RecipeSearchServiceModel>();

            if (inputModel.NeededTime != null)
            {
                serviceModel.NeededTime = this.GetEnum(inputModel.NeededTime, typeof(Period));
            }

            foreach (var allergenName in inputModel.AllergenNames)
            {
                serviceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            var searchedRecipesViewModel = await (await this.recipeService
                .GetAllBySearch(serviceModel))
                .To<RecipeSearchViewModel>()
                .ToListAsync();

            return this.View(searchedRecipesViewModel);
        }

        private Period GetEnum(string description, Type typeOfEnum)
        {
            return (Period)Enum.Parse(
                            typeOfEnum,
                            this.stringFormatService.RemoveWhiteSpaces(description));
        }

        private string GetEnumDescription(string name, Type typeOfEnum)
        {
            FieldInfo specificField = typeOfEnum.GetField(name);

            if (specificField != null)
            {
                DescriptionAttribute attr =
                       Attribute.GetCustomAttribute(
                           specificField,
                           typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attr != null)
                {
                    return attr.Description;
                }
            }

            return null;
        }

        private async Task<RecipeViewDataModel> GetRecipeViewDataModel()
        {
            var categoryTitles = await this.categoryService.GetAllTitles().ToListAsync();
            var allergenNames = await this.allergenService.GetAllNames().ToListAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypes().ToListAsync();
            var periodNames = Enum.GetNames(typeof(Period));
            var levelNames = Enum.GetNames(typeof(Level));
            var sizeNames = Enum.GetNames(typeof(Size));

            var periodDescriptions = new List<string>();
            foreach (var periodName in periodNames)
            {
                periodDescriptions.Add(this.GetEnumDescription(periodName, typeof(Period)));
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
