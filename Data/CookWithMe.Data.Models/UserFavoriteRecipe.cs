namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserFavoriteRecipe
    {
        public UserFavoriteRecipe()
        {
            this.AddedOn = DateTime.UtcNow;
        }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
