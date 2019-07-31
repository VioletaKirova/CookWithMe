namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUserServiceModel : IdentityUser, IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Biography { get; set; }

        public string ProfilePhoto { get; set; }

        public int? LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public ICollection<UserAllergenServiceModel> Allergies { get; set; }

        public ICollection<RecipeServiceModel> Recipes { get; set; }

        public ICollection<UserFavoriteRecipeServiceModel> FavoriteRecipes { get; set; }

        public ICollection<UserCookedRecipeServiceModel> CookedRecipes { get; set; }

        public ICollection<UserCookLaterRecipeServiceModel> CookLaterRecipes { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }
    }
}
