namespace CookWithMe.Web.ViewModels.Categories.Recipes
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryRecipesRecipeViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
