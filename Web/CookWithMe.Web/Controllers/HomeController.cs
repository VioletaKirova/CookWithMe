namespace CookWithMe.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Web.ViewModels.Home.Index;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IRecipeService recipeService;

        public HomeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
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
            var recipes = (await this.recipeService.GetAllFiltered(userId))
                .To<RecipeHomeViewModel>();

            return this.View(recipes);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
