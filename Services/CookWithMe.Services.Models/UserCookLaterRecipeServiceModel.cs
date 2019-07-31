namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserCookLaterRecipeServiceModel : IMapTo<UserCookLaterRecipe>, IMapFrom<UserCookLaterRecipe>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
