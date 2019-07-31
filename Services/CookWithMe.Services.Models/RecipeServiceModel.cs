﻿namespace CookWithMe.Services.Models
{
    using System;
    using System.Collections.Generic;

    using CookWithMe.Data.Models;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Mapping;

    public class RecipeServiceModel : IMapTo<Recipe>, IMapFrom<Recipe>
    {
        public RecipeServiceModel()
        {
            this.Allergens = new HashSet<RecipeAllergenServiceModel>();
        }

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public int CategoryId { get; set; }

        public CategoryServiceModel Category { get; set; }

        public string Summary { get; set; }

        public string Directions { get; set; }

        public string ShoppingListId { get; set; }

        public ShoppingListServiceModel ShoppingList { get; set; }

        public ICollection<RecipeAllergenServiceModel> Allergens { get; set; }

        public int LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public Level SkillLevel { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        public string NutritionalValueId { get; set; }

        public NutritionalValueServiceModel NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public ICollection<UserFavoriteRecipeServiceModel> FavoritedBy { get; set; }

        public ICollection<UserCookedRecipeServiceModel> CookedBy { get; set; }

        public ICollection<ReviewServiceModel> Reviews { get; set; }

        public ICollection<MealRecipeServiceModel> Meals { get; set; }
    }
}
