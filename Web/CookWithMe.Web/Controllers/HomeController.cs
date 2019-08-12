namespace CookWithMe.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.ViewModels;
    using CookWithMe.Web.ViewModels.Home.Index;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly ILifestyleService lifestyleService;
        private readonly IStringFormatService stringFormatService;

        public HomeController(
            IRecipeService recipeService,
            IUserService userService,
            ILifestyleService lifestyleService,
            IStringFormatService stringFormatService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.lifestyleService = lifestyleService;
            this.stringFormatService = stringFormatService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction(nameof(this.IndexLoggedIn));
            }

            return this.View();
        }

        public async Task<IActionResult> IndexLoggedIn(int? pageNumber)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userLifestyleId = (await this.userService.GetByIdAsync(userId)).LifestyleId;

            if (userLifestyleId != null)
            {
                this.ViewData["Lifestyle"] = (await this.lifestyleService.GetByIdAsync(userLifestyleId.Value)).Type;
            }

            var filteredRecipes = (await this.recipeService.GetAllFilteredAsync(userId))
                .To<RecipeHomeViewModel>();

            return this.View(await PaginatedList<RecipeHomeViewModel>
                .CreateAsync(filteredRecipes, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorViewModel = new ErrorViewModel();

            if (this.TempData["ErrorParams"] is Dictionary<string, string> dict)
            {
                errorViewModel.StatusCode = dict["StatusCode"];
                errorViewModel.RequestId = dict["RequestId"];
                errorViewModel.RequestPath = dict["RequestPath"];
            }

            return this.View(errorViewModel);
        }
    }
}
