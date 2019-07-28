namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MealRecipe
    {
        [Required]
        public string MealId { get; set; }

        public Meal Meal { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
