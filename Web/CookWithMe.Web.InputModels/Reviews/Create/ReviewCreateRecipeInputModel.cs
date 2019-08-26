namespace CookWithMe.Web.InputModels.Reviews.Create
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class ReviewCreateRecipeInputModel : IMapTo<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
