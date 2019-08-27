namespace CookWithMe.Web.ViewModels.Users.Profile
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class UserProfileCookedRecipesRecipeViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }
    }
}
