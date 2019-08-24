namespace CookWithMe.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;
    using CookWithMe.Data.Models.Enums;

    public class Recipe : BaseDeletableModel<string>
    {
        public Recipe()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Allergens = new HashSet<RecipeAllergen>();
            this.Lifestyles = new HashSet<RecipeLifestyle>();
            this.FavoritedBy = new HashSet<UserFavoriteRecipe>();
            this.CookedBy = new HashSet<UserCookedRecipe>();
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Photo { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Required]
        [MaxLength(200)]
        public string Summary { get; set; }

        [Required]
        public string Directions { get; set; }

        public string ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }

        public ICollection<RecipeAllergen> Allergens { get; set; }

        public ICollection<RecipeLifestyle> Lifestyles { get; set; }

        public Level SkillLevel { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        public string NutritionalValueId { get; set; }

        public NutritionalValue NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<UserFavoriteRecipe> FavoritedBy { get; set; }

        public ICollection<UserCookedRecipe> CookedBy { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
