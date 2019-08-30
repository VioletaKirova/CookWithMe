namespace CookWithMe.Web.InputModels.Recipes.Edit
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.NutritionalValues;

    public class RecipeEditNutritionalValueInputModel : IMapTo<NutritionalValueServiceModel>, IMapFrom<NutritionalValueServiceModel>
    {
        [Display(Name = "Calories")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Calories { get; set; }

        [Display(Name = "Fats")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Fats { get; set; }

        [Display(Name = "Saturated Fats")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? SaturatedFats { get; set; }

        [Display(Name = "Carbohydrates")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Carbohydrates { get; set; }

        [Display(Name = "Sugar")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Sugar { get; set; }

        [Display(Name = "Protein")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Protein { get; set; }

        [Display(Name = "Fiber")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Fiber { get; set; }

        [Display(Name = "Salt")]
        [Range(AttributesConstraints.NutritionalValueMinValue, AttributesConstraints.NutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Salt { get; set; }
    }
}
