namespace CookWithMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Web.InputModels.Users;
    using CookWithMe.Web.ViewModels.Users;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly IAllergenService allergenService;
        private readonly ILifestyleService lifestyleService;
        private readonly ICloudinaryService cloudinaryService;

        public UsersController(
            IUserService userService,
            IAllergenService allergenService,
            ILifestyleService lifestyleService,
            ICloudinaryService cloudinaryService)
        {
            this.userService = userService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> AddAdditionalInfo()
        {
            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userFullName = (await this.userService.GetByIdAsync(userId)).FullName;

            var userAddAdditionalInfoInputModel = new UserAddAdditionalInfoInputModel
            {
                FullName = userFullName,
            };

            return this.View(userAddAdditionalInfoInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdditionalInfo(UserAddAdditionalInfoInputModel userAddAdditionalInfoInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: Refactor this
                this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

                return this.View();
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

            await this.userService.AddAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);

            return this.Redirect("/");
        }

        [HttpGet]
        public async Task<IActionResult> EditAdditionalInfo()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userServiceModel = await this.userService.GetByIdAsync(userId);

            if (!userServiceModel.HasAdditionalInfo)
            {
                return this.RedirectToAction("AddAdditionalInfo");
            }

            // TODO: Refactor this
            this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

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

            return this.View(userEditAdditionalInfoInputModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdditionalInfo(UserEditAdditionalInfoInputModel userEditAdditionalInfoInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                // TODO: Refactor this
                this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

                return this.View();
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

            await this.userService.EditAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);

            return this.Redirect("/");
        }

        private async Task<UserAdditionalInfoViewModel> GetUserAdditionalInfoViewDataModel()
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
