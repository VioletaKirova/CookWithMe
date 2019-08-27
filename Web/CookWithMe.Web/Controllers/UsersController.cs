namespace CookWithMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services;
    using CookWithMe.Services.Data.Administrators;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;
    using CookWithMe.Services.Models.Users;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.InputModels.Users.AddAdditionalInfo;
    using CookWithMe.Web.InputModels.Users.EditAdditionalInfo;
    using CookWithMe.Web.ViewModels.Users.AdditionalInfo;
    using CookWithMe.Web.ViewModels.Users.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class UsersController : BaseController
    {
        private const string AddAdditionalInfoErrorMessage = "Failed to add the additional info.";
        private const string AddAdditionalInfoSuccessMessage = "You successfully added the additional info.";
        private const string EditAdditionalInfoErrorMessage = "Failed to edit the additional info.";
        private const string EditAdditionalInfoSuccessMessage = "You successfully edited the additional info.";

        private readonly IUserService userService;
        private readonly IAdministratorService administratorService;
        private readonly IRecipeService recipeService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly IUserFavoriteRecipeService userFavoriteRecipeService;
        private readonly IUserCookedRecipeService userCookedRecipeService;
        private readonly ICloudinaryService cloudinaryService;

        public UsersController(
            IUserService userService,
            IAdministratorService administratorService,
            IRecipeService recipeService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            IUserFavoriteRecipeService userFavoriteRecipeService,
            IUserCookedRecipeService userCookedRecipeService,
            ICloudinaryService cloudinaryService)
        {
            this.userService = userService;
            this.administratorService = administratorService;
            this.recipeService = recipeService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.userFavoriteRecipeService = userFavoriteRecipeService;
            this.userCookedRecipeService = userCookedRecipeService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> AddAdditionalInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullName = (await this.userService.GetByIdAsync(userId)).FullName;

            var userAddAdditionalInfoInputModel = new UserAddAdditionalInfoInputModel
            {
                FullName = userFullName,
            };

            userAddAdditionalInfoInputModel
                .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

            return this.View(userAddAdditionalInfoInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdditionalInfo(UserAddAdditionalInfoInputModel userAddAdditionalInfoInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                userAddAdditionalInfoInputModel
                    .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

                return this.View(userAddAdditionalInfoInputModel);
            }

            var userAdditionalInfoServiceModel = userAddAdditionalInfoInputModel
                .To<UserAdditionalInfoServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userAddAdditionalInfoInputModel.ProfilePhoto != null)
            {
                string photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                userAddAdditionalInfoInputModel.ProfilePhoto,
                userId,
                GlobalConstants.CloudFolderForUserProfilePhotos);
                userAdditionalInfoServiceModel.ProfilePhoto = photoUrl;
            }

            foreach (var allergenName in userAddAdditionalInfoInputModel.AllergenNames)
            {
                userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            if (!await this.userService.AddAdditionalInfoAsync(userId, userAdditionalInfoServiceModel))
            {
                this.TempData["Error"] = AddAdditionalInfoErrorMessage;

                userAddAdditionalInfoInputModel
                    .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

                return this.View(userAddAdditionalInfoInputModel);
            }

            this.TempData["Success"] = AddAdditionalInfoSuccessMessage;
            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> EditAdditionalInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userServiceModel = await this.userService.GetByIdAsync(userId);

            if (!userServiceModel.HasAdditionalInfo)
            {
                return this.RedirectToAction(nameof(this.AddAdditionalInfo));
            }

            var userAdditionalInfoServiceModel = await this.userService
                .GetAdditionalInfoByUserIdAsync(userServiceModel.Id);
            var userEditAdditionalInfoInputModel = userAdditionalInfoServiceModel
                .To<UserEditAdditionalInfoInputModel>();

            var allergenNamesViewModel = new List<string>();
            foreach (var userAllergen in userAdditionalInfoServiceModel.Allergies)
            {
                allergenNamesViewModel.Add(userAllergen.Allergen.Name);
            }

            userEditAdditionalInfoInputModel.AllergenNames = allergenNamesViewModel;

            userEditAdditionalInfoInputModel
                .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

            return this.View(userEditAdditionalInfoInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdditionalInfo(UserEditAdditionalInfoInputModel userEditAdditionalInfoInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                userEditAdditionalInfoInputModel
                    .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

                return this.View(userEditAdditionalInfoInputModel);
            }

            var userAdditionalInfoServiceModel = userEditAdditionalInfoInputModel
                .To<UserAdditionalInfoServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userEditAdditionalInfoInputModel.ProfilePhoto != null)
            {
                string photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                userEditAdditionalInfoInputModel.ProfilePhoto,
                userId,
                GlobalConstants.CloudFolderForUserProfilePhotos);
                userAdditionalInfoServiceModel.ProfilePhoto = photoUrl;
            }

            foreach (var allergenName in userEditAdditionalInfoInputModel.AllergenNames)
            {
                userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel { Name = allergenName },
                });
            }

            if (!await this.userService.EditAdditionalInfoAsync(userId, userAdditionalInfoServiceModel))
            {
                this.TempData["Error"] = EditAdditionalInfoErrorMessage;

                userEditAdditionalInfoInputModel
                    .UserAdditionalInfoViewData = await this.GetUserAdditionalInfoViewDataModelAsync();

                return this.View(userEditAdditionalInfoInputModel);
            }

            this.TempData["Success"] = EditAdditionalInfoSuccessMessage;
            return this.Redirect("/");
        }

        [HttpGet]
        [Route("/Users/Profile/Favorite/{id}")]
        public async Task<IActionResult> ProfileFavoriteRecipes(string id, int? pageNumber)
        {
            // TODO: Refactor this
            this.ViewData["UserId"] = id;

            if (await this.administratorService.IsInAdministratorRoleAsync(id))
            {
                this.ViewData["IsAdmin"] = true;
            }

            var favoriteRecipes = this.userFavoriteRecipeService
                .GetRecipesByUserId(id)
                .To<UserProfileFavoriteRecipeViewModel>();

            return this.View(await PaginatedList<UserProfileFavoriteRecipeViewModel>
                .CreateAsync(favoriteRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [HttpGet]
        [Route("/Users/Profile/Cooked/{id}")]
        public async Task<IActionResult> ProfileCookedRecipes(string id, int? pageNumber)
        {
            // TODO: Refactor this
            this.ViewData["UserId"] = id;

            if (await this.administratorService.IsInAdministratorRoleAsync(id))
            {
                this.ViewData["IsAdmin"] = true;
            }

            var cookedRecipes = this.userCookedRecipeService
                .GetRecipesByUserId(id)
                .To<UserProfileCookedRecipeViewModel>();

            return this.View(await PaginatedList<UserProfileCookedRecipeViewModel>
                .CreateAsync(cookedRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [HttpGet]
        [Route("/Users/Profile/Admin/{id}")]
        public async Task<IActionResult> ProfileAdminRecipes(string id, int? pageNumber)
        {
            // TODO: Refactor this
            this.ViewData["UserId"] = id;

            if (await this.administratorService.IsInAdministratorRoleAsync(id))
            {
                this.ViewData["IsAdmin"] = true;
            }

            var adminRecipes = this.recipeService
                .GetByUserId(id)
                .To<UserProfileAdminRecipeViewModel>();

            return this.View(await PaginatedList<UserProfileAdminRecipeViewModel>
                .CreateAsync(adminRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        private async Task<UserAdditionalInfoViewModel> GetUserAdditionalInfoViewDataModelAsync()
        {
            var allergyNames = await this.allergenService.GetAllNamesAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypesAsync();

            var userAdditionalInfoViewModel = new UserAdditionalInfoViewModel
            {
                Allergies = allergyNames,
                Lifestyles = lifestyleTypes,
            };

            return userAdditionalInfoViewModel;
        }
    }
}
