namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class RecipeAllergenServiceModel : IMapTo<RecipeAllergen>, IMapFrom<RecipeAllergen>
    {
        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public int AllergenId { get; set; }

        public AllergenServiceModel Allergen { get; set; }
    }
}
