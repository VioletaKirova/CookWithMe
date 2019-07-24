namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public Review()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        [Required]
        [MaxLength(250)]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        [Required]
        public string ReviewerId { get; set; }

        public ApplicationUser Reviewer { get; set; }
    }
}
