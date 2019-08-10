namespace CookWithMe.Services.Models.Recipes
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Models.Categories;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.NutritionalValues;

    public class RecipeBrowseServiceModel
    {
        public RecipeBrowseServiceModel()
        {
            this.Allergens = new HashSet<RecipeAllergenServiceModel>();
        }

        public CategoryServiceModel Category { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }

        public string KeyWords { get; set; }

        public Level? SkillLevel { get; set; }

        public Size? Serving { get; set; }

        public Period? NeededTime { get; set; }

        public NutritionalValueServiceModel NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public ICollection<RecipeAllergenServiceModel> Allergens { get; set; }
    }
}
