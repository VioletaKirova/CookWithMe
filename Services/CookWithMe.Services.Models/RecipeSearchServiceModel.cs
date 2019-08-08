namespace CookWithMe.Services.Models
{
    using System.Collections.Generic;

    using CookWithMe.Data.Models.Enums;

    public class RecipeSearchServiceModel
    {
        public RecipeSearchServiceModel()
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
