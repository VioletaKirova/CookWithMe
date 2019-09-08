namespace CookWithMe.Web.Areas.Administration.Controllers
{
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

    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : AdministrationController
    {
        private const string CreateErrorMessage = "Failed to create the recipe.";
        private const string EditErrorMessage = "Failed to edit the recipe.";
        private const string EditSuccessMessage = "You successfully edited the recipe!";
        private const string DeleteErrorMessage = "Failed to delete the recipe.";
        private const string DeleteSuccessMessage = "You successfully deleted recipe {0} (ID: {1})!";

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
            var recipeCreateInputModel = new RecipeCreateInputModel()
            {
                RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync(),
            };

            return this.View(recipeCreateInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeCreateInputModel recipeCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                recipeCreateInputModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();

                return this.View(recipeCreateInputModel);
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
                this.TempData["Error"] = CreateErrorMessage;

                recipeCreateInputModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();

                return this.View(recipeCreateInputModel);
            }

            return this.Redirect(nameof(this.All));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);
            var recipeEditInputModel = recipeServiceModel.To<RecipeEditInputModel>();

            recipeEditInputModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();
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
                recipeEditInputModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();
                recipeEditInputModel.NeededTime = this.enumParseService
                    .GetEnumDescription(recipeEditInputModel.NeededTime, typeof(Period));

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
                this.TempData["Error"] = EditErrorMessage;

                recipeEditInputModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();
                recipeEditInputModel.NeededTime = this.enumParseService
                    .GetEnumDescription(recipeEditInputModel.NeededTime, typeof(Period));

                return this.View(recipeEditInputModel);
            }

            this.TempData["Success"] = EditSuccessMessage;

            return this.Redirect($"/Recipes/Details/{id}");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var recipeServiceModel = await this.recipeService.GetByIdAsync(id);
            var recipeDeleteViewModel = recipeServiceModel.To<RecipeDeleteViewModel>();

            recipeDeleteViewModel.RecipeViewData = await this.recipeService.GetRecipeViewDataModelAsync();
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
            var recipeTitle = (await this.recipeService.GetByIdAsync(id)).Title;

            if (!await this.recipeService.DeleteByIdAsync(id))
            {
                this.TempData["Error"] = DeleteErrorMessage;

                return this.Redirect($"/Recipes/Delete/{id}");
            }

            this.TempData["Success"] = string.Format(DeleteSuccessMessage, recipeTitle, id);

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
    }
}
