namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserCookedRecipeServiceModel : IMapTo<UserCookedRecipe>, IMapFrom<UserCookedRecipe>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
