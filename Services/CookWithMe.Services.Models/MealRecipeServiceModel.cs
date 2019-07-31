namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class MealRecipeServiceModel : IMapTo<MealRecipe>, IMapFrom<MealRecipe>
    {
        public string MealId { get; set; }

        public MealServiceModel Meal { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
