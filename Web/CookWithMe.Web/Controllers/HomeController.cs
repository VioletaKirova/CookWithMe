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

    public class HomeController : BaseController
    {
        private readonly IRecipeService recipeService;
        private readonly IStringFormatService stringFormatService;

        public HomeController(IRecipeService recipeService, IStringFormatService stringFormatService)
        {
            this.recipeService = recipeService;
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
            var filteredRecipes = (await this.recipeService.GetAllFiltered(userId))
                .To<RecipeHomeViewModel>()
                .ToList();

            foreach (var recipe in filteredRecipes)
            {
                recipe.FormatedPreparationAndCookingTime = this.stringFormatService
                    .DisplayTime(recipe.PreparationTime + recipe.CookingTime);
            }

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
