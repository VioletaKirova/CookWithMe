namespace CookWithMe.Data.Models
{
    using System;
    using System.Collections.Generic;

    using CookWithMe.Data.Common.Models;

    public class Meal : BaseDeletableModel<string>
    {
        public Meal()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.IsDeleted = false;
            this.Recipes = new HashSet<MealRecipe>();
        }

        public ICollection<MealRecipe> Recipes { get; set; }
    }
}
