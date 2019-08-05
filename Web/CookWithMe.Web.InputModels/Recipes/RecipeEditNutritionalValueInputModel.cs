namespace CookWithMe.Web.InputModels.Recipes
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeEditNutritionalValueInputModel : IMapTo<NutritionalValueServiceModel>, IMapFrom<NutritionalValueServiceModel>
    {
        public double? Calories { get; set; }

        public double? Fats { get; set; }

        public double? SaturatedFats { get; set; }

        public double? Carbohydrates { get; set; }

        public double? Sugar { get; set; }

        public double? Protein { get; set; }

        public double? Fiber { get; set; }

        public double? Salt { get; set; }
    }
}
