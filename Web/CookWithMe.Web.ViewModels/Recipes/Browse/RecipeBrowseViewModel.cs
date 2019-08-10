namespace CookWithMe.Web.ViewModels.Recipes.Browse
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class RecipeBrowseViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
