namespace CookWithMe.Web.ViewComponents
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ProfileBarViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly ILifestyleService lifestyleService;

        public ProfileBarViewComponent(IUserService userService, ILifestyleService lifestyleService)
        {
            this.userService = userService;
            this.lifestyleService = lifestyleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userName)
        {
            var id = await this.userService.GetIdByUserNameAsync(userName);
            var userServiceModel = await this.userService.GetByIdAsync(id);

            var viewModel = userServiceModel
                .To<ProfileBarViewComponentViewModel>();

            viewModel.UserName = userName;
            if (userServiceModel.LifestyleId != null)
            {
                viewModel.LifestyleType = (await this.lifestyleService.GetByIdAsync(userServiceModel.LifestyleId.Value)).Type;
            }

            return this.View(viewModel);
        }
    }
}
