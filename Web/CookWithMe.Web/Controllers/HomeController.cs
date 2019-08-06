namespace CookWithMe.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services;
    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Home.Index;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

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
                return this.RedirectToAction("IndexLoggedIn");
            }

            return this.View();
        }

        public async Task<IActionResult> IndexLoggedIn()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userLifestyleId = (await this.userService.GetById(userId)).LifestyleId;
            this.ViewData["Lifestyle"] = (await this.lifestyleService.GetById(userLifestyleId.Value))?.Type;

            var filteredRecipes = await (await this.recipeService.GetAllFiltered(userId))
                .To<RecipeHomeViewModel>()
                .ToListAsync();

            return this.View(filteredRecipes);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
