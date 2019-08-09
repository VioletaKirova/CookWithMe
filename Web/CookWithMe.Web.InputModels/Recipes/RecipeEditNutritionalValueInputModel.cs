namespace CookWithMe.Web.InputModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeEditNutritionalValueInputModel : IMapTo<NutritionalValueServiceModel>, IMapFrom<NutritionalValueServiceModel>
    {
        [Display(Name = "Calories")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Calories { get; set; }

        [Display(Name = "Fats")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Fats { get; set; }

        [Display(Name = "Saturated Fats")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? SaturatedFats { get; set; }

        [Display(Name = "Carbohydrates")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Carbohydrates { get; set; }

        [Display(Name = "Sugar")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Sugar { get; set; }

        [Display(Name = "Protein")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Protein { get; set; }

        [Display(Name = "Fiber")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Fiber { get; set; }

        [Display(Name = "Salt")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public double? Salt { get; set; }
    }
}
