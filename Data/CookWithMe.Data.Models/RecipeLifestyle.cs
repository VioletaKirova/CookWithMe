namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RecipeLifestyle
    {
        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int LifestyleId { get; set; }

        public Lifestyle Lifestyle { get; set; }
    }
}
