namespace CookWithMe.Web.ViewModels.Recipes.Details
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.NutritionalValues;

    public class RecipeDetailsNutritionalValueViewModel : IMapFrom<NutritionalValueServiceModel>
    {
        public string Id { get; set; }

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
