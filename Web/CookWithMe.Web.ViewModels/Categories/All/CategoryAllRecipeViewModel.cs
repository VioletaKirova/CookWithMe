namespace CookWithMe.Web.ViewModels.Categories.All
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryAllRecipeViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
