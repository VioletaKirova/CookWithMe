namespace CookWithMe.Web.ViewComponents
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewComponents.Models;

    using Microsoft.AspNetCore.Mvc;

    public class ProfileSidebarViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        private readonly ILifestyleService lifestyleService;

        public ProfileSidebarViewComponent(IUserService userService, ILifestyleService lifestyleService)
        {
            this.userService = userService;
            this.lifestyleService = lifestyleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var userServiceModel = await this.userService
                .GetByIdAsync(id);

            var viewModel = userServiceModel
                .To<ProfileSidebarViewComponentViewModel>();

            viewModel.Id = id;
            if (userServiceModel.LifestyleId != null)
            {
                viewModel.LifestyleType = (await this.lifestyleService.GetByIdAsync(userServiceModel.LifestyleId.Value)).Type;
            }

            return this.View(viewModel);
        }
    }
}
