namespace CookWithMe.Web.Controllers
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class HomeController : BaseController
    {
        private readonly IRecipeService recipeService;

        public HomeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

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
            var recipes = await this.recipeService.GetAllFiltered(userId);

            // TODO: Map recipes to IEnumerable<RecipeHomeViewModel>

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
