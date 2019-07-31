namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserFavoriteRecipeServiceModel : IMapTo<UserFavoriteRecipe>, IMapFrom<UserFavoriteRecipe>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
