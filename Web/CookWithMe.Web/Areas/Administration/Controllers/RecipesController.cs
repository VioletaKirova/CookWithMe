namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Recipes;
    using CookWithMe.Web.ViewModels.Recipes;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class RecipesController : AdministrationController
    {
        private readonly ICategoryService categoryService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IRecipeService recipeService;
        private readonly IStringFormatService stringFormatService;
        private readonly IEnumParseService enumParserService;

        public RecipesController(
            ICategoryService categoryService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            ICloudinaryService cloudinaryService,
            IRecipeService recipeService,
            IStringFormatService stringFormatService,
            IEnumParseService enumParserService)
        {
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.cloudinaryService = cloudinaryService;
            this.recipeService = recipeService;
            this.stringFormatService = stringFormatService;
            this.enumParserService = enumParserService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                return this.View();
            }

            var recipeServiceModel = model.To<RecipeServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipeServiceModel.UserId = userId;

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                model.Photo,
                $"{userId}-{model.Title}",
                GlobalConstants.CloudFolderForRecipePhotos);
            recipeServiceModel.Photo = photoUrl;

            foreach (var allergenName in model.AllergenNames)
            {
                recipeServiceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            foreach (var lifestyleType in model.LifestyleTypes)
            {
                recipeServiceModel.Lifestyles.Add(new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel { Type = lifestyleType },
                });
            }

            recipeServiceModel.NeededTime = this.enumParserService.Parse<Period>(model.NeededTime, typeof(Period));

            await this.recipeService.CreateAsync(recipeServiceModel);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var recipe = await this.recipeService.GetById(id);
            var recipeViewModel = recipe.To<RecipeEditInputModel>();

            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            recipeViewModel.NeededTime = this.enumParserService.GetEnumDescription(recipeViewModel.NeededTime, typeof(Period));

            var allergenNamesViewModel = new List<string>();
            foreach (var recipeAllergen in recipe.Allergens)
            {
                allergenNamesViewModel.Add(recipeAllergen.Allergen.Name);
            }

            recipeViewModel.AllergenNames = allergenNamesViewModel;

            var lifestyleTypesViewModel = new List<string>();
            foreach (var recipeLifestyle in recipe.Lifestyles)
            {
                lifestyleTypesViewModel.Add(recipeLifestyle.Lifestyle.Type);
            }

            recipeViewModel.LifestyleTypes = lifestyleTypesViewModel;

            return this.View(recipeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RecipeEditInputModel recipeViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                recipeViewModel.NeededTime = this.enumParserService.GetEnumDescription(recipeViewModel.NeededTime, typeof(Period));

                return this.View(recipeViewModel);
            }

            var recipeServiceModel = recipeViewModel.To<RecipeServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipeServiceModel.UserId = userId;

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                recipeViewModel.Photo,
                $"{userId}-{recipeViewModel.Title}",
                GlobalConstants.CloudFolderForRecipePhotos);
            recipeServiceModel.Photo = photoUrl;

            foreach (var allergenName in recipeViewModel.AllergenNames)
            {
                recipeServiceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            foreach (var lifestyleType in recipeViewModel.LifestyleTypes)
            {
                recipeServiceModel.Lifestyles.Add(new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel { Type = lifestyleType },
                });
            }

            recipeServiceModel.NeededTime = this.enumParserService.Parse<Period>(recipeViewModel.NeededTime, typeof(Period));

            await this.recipeService.Edit(id, recipeServiceModel);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var recipe = await this.recipeService.GetById(id);
            var recipeViewModel = recipe.To<RecipeDeleteViewModel>();

            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            recipeViewModel.NeededTime = this.enumParserService.GetEnumDescription(recipeViewModel.NeededTime, typeof(Period));

            var allergenNamesViewModel = new List<string>();
            foreach (var recipeAllergen in recipe.Allergens)
            {
                allergenNamesViewModel.Add(recipeAllergen.Allergen.Name);
            }

            recipeViewModel.AllergenNames = allergenNamesViewModel;

            var lifestyleTypesViewModel = new List<string>();
            foreach (var recipeLifestyle in recipe.Lifestyles)
            {
                lifestyleTypesViewModel.Add(recipeLifestyle.Lifestyle.Type);
            }

            recipeViewModel.LifestyleTypes = lifestyleTypesViewModel;

            return this.View(recipeViewModel);
        }

        [HttpPost]
        [Route("/Administration/Recipes/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            await this.recipeService.Delete(id);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recipes = await this.recipeService.
                GetAllByUserId(userId)
                .To<RecipeAllViewModel>()
                .ToListAsync();

            return this.View(recipes);
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
                periodDescriptions.Add(this.enumParserService.GetEnumDescription(periodName, typeof(Period)));
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
