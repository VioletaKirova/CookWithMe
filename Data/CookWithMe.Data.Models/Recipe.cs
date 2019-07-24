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
            this.FavoritedBy = new HashSet<UserFavoriteRecipe>();
            this.CookedBy = new HashSet<UserCookedRecipe>();
            this.Reviews = new HashSet<Review>();
            this.Meals = new HashSet<MealRecipe>();
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

        [Required]
        public string ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }

        public ICollection<RecipeAllergen> Allergens { get; set; }

        public int LifestyleId { get; set; }

        public Lifestyle Lifestyle { get; set; }

        public Level SkillLevel { get; set; }

        [Required]
        public string PreparationTime { get; set; }

        [Required]
        public string CookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        [Required]
        public string NutritionalValueId { get; set; }

        public NutritionalValue NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public ICollection<UserFavoriteRecipe> FavoritedBy { get; set; }

        public ICollection<UserCookedRecipe> CookedBy { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<MealRecipe> Meals { get; set; }
    }
}
