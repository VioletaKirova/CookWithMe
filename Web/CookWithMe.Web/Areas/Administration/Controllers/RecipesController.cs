﻿namespace CookWithMe.Web.Areas.Administration.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services;
    using CookWithMe.Services.Data;
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

        public RecipesController(ICategoryService categoryService, IAllergenService allergenService, ILifestyleService lifestyleService, ICloudinaryService cloudinaryService, IRecipeService recipeService)
        {
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.cloudinaryService = cloudinaryService;
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryTitles = await this.categoryService.GetAllTitles().ToListAsync();
            var allergenNames = await this.allergenService.GetAllNames().ToListAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypes().ToListAsync();
            var periodValues = Enum.GetNames(typeof(Period));
            var levelValues = Enum.GetNames(typeof(Level));
            var sizeValues = Enum.GetNames(typeof(Size));

            var recipeCreateViewModel = new RecipeCreateViewModel
            {
                CategoryTitles = categoryTitles,
                AllergenNames = allergenNames,
                LifestyleTypes = lifestyleTypes,
                PeriodValues = periodValues,
                LevelValues = levelValues,
                SizeValues = sizeValues,
            };

            this.ViewData["Types"] = recipeCreateViewModel;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var categoryTitles = await this.categoryService.GetAllTitles().ToListAsync();
                var allergenNames = await this.allergenService.GetAllNames().ToListAsync();
                var lifestyleTypes = await this.lifestyleService.GetAllTypes().ToListAsync();
                var periodValues = Enum.GetNames(typeof(Period));
                var levelValues = Enum.GetNames(typeof(Level));
                var sizeValues = Enum.GetNames(typeof(Size));

                var recipeCreateViewModel = new RecipeCreateViewModel
                {
                    CategoryTitles = categoryTitles,
                    AllergenNames = allergenNames,
                    LifestyleTypes = lifestyleTypes,
                    PeriodValues = periodValues,
                    LevelValues = levelValues,
                    SizeValues = sizeValues,
                };

                this.ViewData["Types"] = recipeCreateViewModel;

                return this.View();
            }

            var recipeServiceModel = AutoMapper.Mapper.Map<RecipeCreateInputModel, RecipeServiceModel>(model);

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipeServiceModel.UserId = userId;

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                model.Photo,
                $"{userId}-{model.Title}",
                GlobalConstants.CloudFolderForRecipePhotos);
            recipeServiceModel.Photo = photoUrl;

            await this.recipeService.CreateAsync(recipeServiceModel);

            return this.Redirect("/");
        }
    }
}