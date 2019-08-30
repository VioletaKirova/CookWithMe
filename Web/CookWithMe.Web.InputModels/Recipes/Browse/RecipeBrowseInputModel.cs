namespace CookWithMe.Web.InputModels.Recipes.Browse
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.ViewModels.Recipes.ViewData;

    public class RecipeBrowseInputModel : IMapTo<RecipeBrowseServiceModel>, IHaveCustomMappings
    {
        public RecipeBrowseInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Display(Name = "Category Title")]
        [MaxLength(AttributesConstraints.BrowseSingleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string CategoryTitle { get; set; }

        [Display(Name = "Lifestyle Type")]
        [MaxLength(AttributesConstraints.BrowseSingleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string LifestyleType { get; set; }

        [Display(Name = "Key Words")]
        [MaxLength(AttributesConstraints.BrowseMultipleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string KeyWords { get; set; }

        [Display(Name = "Skill Level")]
        [MaxLength(AttributesConstraints.BrowseSingleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string SkillLevel { get; set; }

        [Display(Name = "Serving")]
        [MaxLength(AttributesConstraints.BrowseSingleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string Serving { get; set; }

        [Display(Name = "Needed Time")]
        [MaxLength(AttributesConstraints.BrowseSingleWordFieldsMaxLength, ErrorMessage = AttributesErrorMessages.MaxLengthErrorMessage)]
        public string NeededTime { get; set; }

        [Display(Name = "Nutritional Value")]
        public RecipeBrowseNutritionalValueInputModel NutritionalValue { get; set; }

        [Display(Name = "Yield")]
        [Range(AttributesConstraints.BrowseYieldMinValue, AttributesConstraints.BrowseYieldMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal? Yield { get; set; }

        [Display(Name = "Allergens")]
        public IEnumerable<string> AllergenNames { get; set; }

        public RecipeViewDataModel RecipeViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeBrowseInputModel, RecipeBrowseServiceModel>()
                .ForMember(
                    destination => destination.Category,
                    opts => opts.MapFrom(origin => new CategoryServiceModel { Title = origin.CategoryTitle }))
                .ForMember(
                    destination => destination.Lifestyle,
                    opts => opts.MapFrom(origin => new LifestyleServiceModel { Type = origin.LifestyleType }))
                .ForMember(
                    destination => destination.NeededTime,
                    opts => opts.Ignore());
        }
    }
}
