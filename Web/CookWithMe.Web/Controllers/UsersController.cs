namespace CookWithMe.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services;
    using CookWithMe.Services.Data;
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
        public async Task<IActionResult> AdditionalInfo()
        {
            var allergyNames = await this.allergenService.GetAllNames().ToListAsync();
            var lifestyleTypes = await this.lifestyleService.GetAllTypes().ToListAsync();

            var userAdditionalInfoViewModel = new UserAdditionalInfoViewModel
            {
                Allergies = allergyNames,
                Lifestyles = lifestyleTypes,
            };

            this.ViewData["Types"] = userAdditionalInfoViewModel;

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AdditionalInfo(UserAdditionalInfoInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            string photoUrl = await this.cloudinaryService.UploadPhotoAsync(
                model.ProfilePhoto,
                userId,
                GlobalConstants.CloudFolderForUserProfilePhotos);

            var lifestyleId = await this.lifestyleService.GetIdByType(model.Lifestyle);

            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel
            {
                Biography = model.Biography,
                ProfilePhoto = photoUrl,
                LifestyleId = lifestyleId,
            };

            if (model.Allergies.Any())
            {
                foreach (var allergy in model.Allergies)
                {
                    var allergenId = await this.allergenService.GetIdByName(allergy);

                    userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel
                    {
                        UserId = userId,
                        AllergenId = allergenId,
                    });
                }
            }

            await this.userService.UpdateUserAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);

            return this.Redirect("/");
        }
    }
}
