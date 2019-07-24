namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserAllergen
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int AllergenId { get; set; }

        public Allergen Allergen { get; set; }
    }
}
