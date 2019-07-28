namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class NutritionalValueServiceModel : IMapTo<NutritionalValue>
    {
        public double? Calories { get; set; }

        public double? Fats { get; set; }

        public double? SaturatedFats { get; set; }

        public double? Carbohydrates { get; set; }

        public double? Sugar { get; set; }

        public double? Protein { get; set; }

        public double? Fiber { get; set; }

        public double? Salt { get; set; }

        public string RecipeId { get; set; }
    }
}
