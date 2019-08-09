namespace CookWithMe.Web.InputModels.Recipes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeSearchInputModel : IMapTo<RecipeSearchServiceModel>, IHaveCustomMappings
    {
        public RecipeSearchInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Display(Name = "Category Title")]
        [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string CategoryTitle { get; set; }

        [Display(Name = "Lifestyle Type")]
        [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string LifestyleType { get; set; }

        [Display(Name = "Key Words")]
        [MaxLength(200, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string KeyWords { get; set; }

        [Display(Name = "Skill Level")]
        [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string SkillLevel { get; set; }

        [Display(Name = "Serving")]
        [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string Serving { get; set; }

        [Display(Name = "Needed Time")]
        [MaxLength(20, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public string NeededTime { get; set; }

        [Display(Name = "Nutritional Value")]
        public RecipeSearchNutritionalValueInputModel NutritionalValue { get; set; }

        [Display(Name = "Yield")]
        [Range(0.1, 10000, ErrorMessage = ErrorMessages.RangeErrorMessage)]
        public decimal? Yield { get; set; }

        [Display(Name = "Allergens")]
        [MaxLength(200, ErrorMessage = ErrorMessages.MaxLengthErrorMessage)]
        public IEnumerable<string> AllergenNames { get; set; }


        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeSearchInputModel, RecipeSearchServiceModel>()
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
