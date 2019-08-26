namespace CookWithMe.Web.InputModels.Reviews.Edit
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class ReviewEditRecipeInputModel : IMapTo<RecipeServiceModel>, IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }
    }
}
