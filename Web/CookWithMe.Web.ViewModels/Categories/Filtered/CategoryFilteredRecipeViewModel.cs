namespace CookWithMe.Web.ViewModels.Categories.Filtered
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class CategoryFilteredRecipeViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
