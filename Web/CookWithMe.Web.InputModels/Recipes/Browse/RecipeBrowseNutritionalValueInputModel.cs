namespace CookWithMe.Web.InputModels.Recipes.Browse
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.NutritionalValues;

    public class RecipeBrowseNutritionalValueInputModel : IMapTo<NutritionalValueServiceModel>
    {
        [Display(Name = "Calories")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Calories { get; set; }

        [Display(Name = "Fats")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Fats { get; set; }

        [Display(Name = "Saturated Fats")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? SaturatedFats { get; set; }

        [Display(Name = "Carbohydrates")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Carbohydrates { get; set; }

        [Display(Name = "Sugar")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Sugar { get; set; }

        [Display(Name = "Protein")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Protein { get; set; }

        [Display(Name = "Fiber")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Fiber { get; set; }

        [Display(Name = "Salt")]
        [Range(AttributesConstraints.BrowseNutritionalValueMinValue, AttributesConstraints.BrowseNutritionalValueMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public double? Salt { get; set; }
    }
}
