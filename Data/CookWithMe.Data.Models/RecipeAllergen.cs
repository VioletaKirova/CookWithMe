namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeAllergen
    {
        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int AllergenId { get; set; }

        public Allergen Allergen { get; set; }
    }
}
