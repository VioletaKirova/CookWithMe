﻿namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Mapping;

    public class RecipeServiceModel : IMapTo<Recipe>
    {
        public RecipeServiceModel()
        {
            this.Allergens = new HashSet<RecipeAllergenServiceModel>();
        }

        public string Title { get; set; }

        public string Photo { get; set; }

        public int CategoryId { get; set; }

        public string Summary { get; set; }

        public string Directions { get; set; }

        public ShoppingListServiceModel ShoppingList { get; set; }

        public ICollection<RecipeAllergenServiceModel> Allergens { get; set; }

        public int LifestyleId { get; set; }

        public Level SkillLevel { get; set; }

        public string PreparationTime { get; set; }

        public string CookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        public NutritionalValueServiceModel NutritionalValueServiceModel { get; set; }

        public decimal? Yield { get; set; }

        public string UserId { get; set; }
    }
}
