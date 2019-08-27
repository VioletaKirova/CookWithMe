namespace CookWithMe.Web.ViewModels.Users.Profile
{
    using System.Collections.Generic;

    public class UserProfileFavoriteRecipesViewModel
    {
        public UserProfileFavoriteRecipesViewModel()
        {
            this.FavoriteRecipes = new List<UserProfileFavoriteRecipesRecipeViewModel>();
        }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public ICollection<UserProfileFavoriteRecipesRecipeViewModel> FavoriteRecipes { get; set; }
    }
}
