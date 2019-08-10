namespace CookWithMe.Services.Models.Recipes
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;

    public class RecipeAllergenServiceModel : IMapTo<RecipeAllergen>, IMapFrom<RecipeAllergen>
    {
        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public int AllergenId { get; set; }

        public AllergenServiceModel Allergen { get; set; }
    }
}
