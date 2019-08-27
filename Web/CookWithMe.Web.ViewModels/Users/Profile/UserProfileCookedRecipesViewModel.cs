namespace CookWithMe.Web.ViewModels.Users.Profile
{
    using System.Collections.Generic;

    public class UserProfileCookedRecipesViewModel
    {
        public UserProfileCookedRecipesViewModel()
        {
            this.CookedRecipes = new List<UserProfileCookedRecipesRecipeViewModel>();
        }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public ICollection<UserProfileCookedRecipesRecipeViewModel> CookedRecipes { get; set; }
    }
}
