namespace CookWithMe.Web.ViewModels.Users.Profile
{
    using System.Collections.Generic;

    public class UserProfileAdminRecipesViewModel
    {
        public UserProfileAdminRecipesViewModel()
        {
            this.AdminRecipes = new List<UserProfileAdminRecipesRecipeViewModel>();
        }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public ICollection<UserProfileAdminRecipesRecipeViewModel> AdminRecipes { get; set; }
    }
}
