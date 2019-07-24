namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class ShoppingList : BaseDeletableModel<string>
    {
        public ShoppingList()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
