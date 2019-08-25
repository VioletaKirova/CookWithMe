namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Common;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.Infrastructure;
    using CookWithMe.Web.ViewModels.Home.Index;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IUserService userService;
        private readonly ILifestyleService lifestyleService;

        public HomeController(
            IRecipeService recipeService,
            IUserService userService,
            ILifestyleService lifestyleService)
        {
            this.recipeService = recipeService;
            this.userService = userService;
            this.lifestyleService = lifestyleService;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction(nameof(this.IndexLoggedIn));
            }

            return this.View();
        }

        [Authorize]
        [HttpGet]
        [Route("/Home/Index")]
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
        public IActionResult Error(int? statusCode = null)
        {
            if (statusCode == StatusCodes.NotFound)
            {
                return this.Redirect($"/Error/{StatusCodes.NotFound}");
            }

            return this.Redirect($"/Error/{StatusCodes.InternalServerError}");
        }
    }
}
