namespace CookWithMe.Services.Models.Meals
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class MealRecipeServiceModel : IMapTo<MealRecipe>, IMapFrom<MealRecipe>
    {
        public string MealId { get; set; }

        public MealServiceModel Meal { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
