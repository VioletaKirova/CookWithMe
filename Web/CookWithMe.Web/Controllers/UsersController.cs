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

        public UsersController(IUserService userService, IAllergenService allergenService, ILifestyleService lifestyleService, ICloudinaryService cloudinaryService)
        {
            this.userService = userService;
            this.allergenService = allergenService;
            this.lifestyleService = lifestyleService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> AddAdditionalInfo()
        {
            this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdditionalInfo(UserAddAdditionalInfoInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

                return this.View();
            }

            var userAdditionalInfoServiceModel = model.To<UserAdditionalInfoServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.ProfilePhoto != null)
            {
                string photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                model.ProfilePhoto,
                userId,
                GlobalConstants.CloudFolderForUserProfilePhotos);
                userAdditionalInfoServiceModel.ProfilePhoto = photoUrl;
            }

            foreach (var allergenName in model.AllergenNames)
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
            var user = await this.userService.GetById(this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!user.HasAdditionalInfo)
            {
                return this.RedirectToAction("AddAdditionalInfo");
            }

            this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

            var userAdditionalInfoServiceModel = await this.userService.GetAdditionalInfo(user.Id);
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
        public async Task<IActionResult> EditAdditionalInfo(UserEditAdditionalInfoInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                this.ViewData["Model"] = await this.GetUserAdditionalInfoViewDataModel();

                return this.View(model);
            }

            var userAdditionalInfoServiceModel = model.To<UserAdditionalInfoServiceModel>();

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.ProfilePhoto != null)
            {
                string photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                model.ProfilePhoto,
                userId,
                GlobalConstants.CloudFolderForUserProfilePhotos);
                userAdditionalInfoServiceModel.ProfilePhoto = photoUrl;
            }

            foreach (var allergenName in model.AllergenNames)
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
            var allergyNames = await this.allergenService.GetAllNames().ToListAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypes().ToListAsync();

            var userAdditionalInfoViewModel = new UserAdditionalInfoViewModel
            {
                Allergies = allergyNames,
                Lifestyles = lifestyleTypes,
            };

            return userAdditionalInfoViewModel;
        }
    }
}
