namespace CookWithMe.Web.ViewModels.Recipes
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeDetailsApplicationUserViewModel : IMapFrom<ApplicationUserServiceModel>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }
    }
}
