namespace CookWithMe.Web.Areas.Administration.Controllers
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
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.InputModels.Recipes.Create;
    using CookWithMe.Web.InputModels.Recipes.Edit;
    using CookWithMe.Web.ViewModels.Recipes.All;
    using CookWithMe.Web.ViewModels.Recipes.Delete;
    using CookWithMe.Web.ViewModels.Recipes.ViewData;

    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : AdministrationController
    {
        private readonly ICategoryService categoryService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly IRecipeService recipeService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IEnumParseService enumParseService;

        public RecipesController(
            ICategoryService categoryService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            IRecipeService recipeService,
            ICloudinaryService cloudinaryService,
            IEnumParseService enumParseService)
        {
            this.categoryService = categoryService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.recipeService = recipeService;
            this.cloudinaryService = cloudinaryService;
            this.enumParseService = enumParseService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateInputModel recipeCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: Refactor this
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                return this.View();
            }

            var recipeServiceModel = recipeCreateInputModel.To<RecipeServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipeServiceModel.UserId = userId;

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                recipeCreateInputModel.Photo,
                $"{userId}-{recipeCreateInputModel.Title}",
                GlobalConstants.CloudFolderForRecipePhotos);
            recipeServiceModel.Photo = photoUrl;

            foreach (var allergenName in recipeCreateInputModel.AllergenNames)
            {
                recipeServiceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            foreach (var lifestyleType in recipeCreateInputModel.LifestyleTypes)
            {
                recipeServiceModel.Lifestyles.Add(new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel { Type = lifestyleType },
                });
            }

            recipeServiceModel.NeededTime = this.enumParseService
                .Parse<Period>(recipeCreateInputModel.NeededTime);

            if (!await this.recipeService.CreateAsync(recipeServiceModel))
            {
                return this.Redirect($"/Home/Error?statusCode={StatusCodes.InternalServerError}&id={this.HttpContext.TraceIdentifier}");
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);
            var recipeEditInputModel = recipeServiceModel.To<RecipeEditInputModel>();

            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            recipeEditInputModel.NeededTime = this.enumParseService
                .GetEnumDescription(recipeEditInputModel.NeededTime, typeof(Period));

            var allergenNamesViewModel = new List<string>();
            foreach (var recipeAllergen in recipeServiceModel.Allergens)
            {
                allergenNamesViewModel.Add(recipeAllergen.Allergen.Name);
            }

            recipeEditInputModel.AllergenNames = allergenNamesViewModel;

            var lifestyleTypesViewModel = new List<string>();
            foreach (var recipeLifestyle in recipeServiceModel.Lifestyles)
            {
                lifestyleTypesViewModel.Add(recipeLifestyle.Lifestyle.Type);
            }

            recipeEditInputModel.LifestyleTypes = lifestyleTypesViewModel;

            return this.View(recipeEditInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RecipeEditInputModel recipeEditInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: Refactor this
                this.ViewData["Model"] = await this.GetRecipeViewDataModel();

                recipeEditInputModel.NeededTime = this.enumParseService.GetEnumDescription(recipeEditInputModel.NeededTime, typeof(Period));

                return this.View(recipeEditInputModel);
            }

            var recipeServiceModel = recipeEditInputModel.To<RecipeServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            recipeServiceModel.UserId = userId;

            var photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                recipeEditInputModel.Photo,
                $"{userId}-{recipeEditInputModel.Title}",
                GlobalConstants.CloudFolderForRecipePhotos);
            recipeServiceModel.Photo = photoUrl;

            foreach (var allergenName in recipeEditInputModel.AllergenNames)
            {
                recipeServiceModel.Allergens.Add(new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            foreach (var lifestyleType in recipeEditInputModel.LifestyleTypes)
            {
                recipeServiceModel.Lifestyles.Add(new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel { Type = lifestyleType },
                });
            }

            recipeServiceModel.NeededTime = this.enumParseService
                .Parse<Period>(recipeEditInputModel.NeededTime);

            if (!await this.recipeService.EditAsync(id, recipeServiceModel))
            {
                return this.Redirect($"/Home/Error?statusCode={StatusCodes.InternalServerError}&id={this.HttpContext.TraceIdentifier}");
            }

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);
            var recipeDeleteViewModel = recipeServiceModel.To<RecipeDeleteViewModel>();

            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetRecipeViewDataModel();

            recipeDeleteViewModel.NeededTime = this.enumParseService
                .GetEnumDescription(recipeDeleteViewModel.NeededTime, typeof(Period));

            var allergenNames = new List<string>();
            foreach (var recipeAllergen in recipeServiceModel.Allergens)
            {
                allergenNames.Add(recipeAllergen.Allergen.Name);
            }

            recipeDeleteViewModel.AllergenNames = allergenNames;

            var lifestyleTypes = new List<string>();
            foreach (var recipeLifestyle in recipeServiceModel.Lifestyles)
            {
                lifestyleTypes.Add(recipeLifestyle.Lifestyle.Type);
            }

            recipeDeleteViewModel.LifestyleTypes = lifestyleTypes;

            return this.View(recipeDeleteViewModel);
        }

        [HttpPost]
        [Route("/Administration/Recipes/Delete/{id}")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            if (!await this.recipeService.DeleteByIdAsync(id))
            {
                return this.Redirect($"/Home/Error?statusCode={StatusCodes.InternalServerError}&id={this.HttpContext.TraceIdentifier}");
            }

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> All(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var allRecipesByAdmin = this.recipeService.
                GetByUserId(userId)
                .To<RecipeAllViewModel>();

            return this.View(await PaginatedList<RecipeAllViewModel>
                .CreateAsync(allRecipesByAdmin, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
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
                periodDescriptions.Add(this.enumParseService.GetEnumDescription(periodName, typeof(Period)));
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
