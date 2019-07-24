namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserCookedRecipe
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
