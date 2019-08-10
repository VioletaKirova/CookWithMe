namespace CookWithMe.Services.Models.Users
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Services.Models.Reviews;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUserServiceModel : IdentityUser, IMapFrom<ApplicationUser>
    {
        public ApplicationUserServiceModel()
        {
            this.Allergies = new HashSet<UserAllergenServiceModel>();
            this.Recipes = new HashSet<RecipeServiceModel>();
            this.FavoriteRecipes = new HashSet<UserFavoriteRecipeServiceModel>();
            this.CookedRecipes = new HashSet<UserCookedRecipeServiceModel>();
            this.UserShoppingLists = new HashSet<UserShoppingListServiceModel>();
            this.Reviews = new HashSet<ReviewServiceModel>();
        }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public int? LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public bool HasAdditionalInfo { get; set; }

        public ICollection<UserAllergenServiceModel> Allergies { get; set; }

        public ICollection<RecipeServiceModel> Recipes { get; set; }

        public ICollection<UserFavoriteRecipeServiceModel> FavoriteRecipes { get; set; }

        public ICollection<UserCookedRecipeServiceModel> CookedRecipes { get; set; }

        public ICollection<UserShoppingListServiceModel> UserShoppingLists { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }
    }
}
