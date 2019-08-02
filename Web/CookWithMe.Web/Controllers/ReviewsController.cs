namespace CookWithMe.Web.Controllers
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Data;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class ReviewsController : BaseController
    {
        private readonly IRecipeService recipeService;

        public ReviewsController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(string id)
        {
            var recipeServiceModel = await this.recipeService.GetById(id);

            this.ViewData["RecipeTitle"] = recipeServiceModel.Title;

            return this.View();
        }
    }
}
