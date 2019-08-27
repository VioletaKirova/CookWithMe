namespace CookWithMe.Web.ViewModels.Recipes.Details
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Users;

    public class RecipeDetailsApplicationUserViewModel : IMapFrom<ApplicationUserServiceModel>
    {
        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }
    }
}
