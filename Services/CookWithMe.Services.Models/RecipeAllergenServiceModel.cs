namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class RecipeAllergenServiceModel : IMapTo<RecipeAllergen>
    {
        public int AllergenId { get; set; }
    }
}
