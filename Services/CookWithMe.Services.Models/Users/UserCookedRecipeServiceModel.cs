namespace CookWithMe.Services.Models.Users
{
    using System;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class UserCookedRecipeServiceModel : IMapTo<UserCookedRecipe>, IMapFrom<UserCookedRecipe>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
