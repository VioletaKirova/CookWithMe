namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Data.Common.Models;

    public class NutritionalValue : BaseDeletableModel<string>
    {
        public NutritionalValue()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
        }

        public double? Calories { get; set; }

        public double? Fats { get; set; }

        public double? SaturatedFats { get; set; }

        public double? Carbohydrates { get; set; }

        public double? Sugar { get; set; }

        public double? Protein { get; set; }

        public double? Fiber { get; set; }

        public double? Salt { get; set; }

        [Required]
        public string RecipeId { get; set; }

        public Recipe Recipe { get; set; }
    }
}
