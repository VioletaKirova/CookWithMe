namespace CookWithMe.Web.InputModels.Recipes.Create
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.Common.ValidationAttribures;
    using CookWithMe.Web.ViewModels.Recipes.ViewData;

    using Microsoft.AspNetCore.Http;

    public class RecipeCreateInputModel : IMapTo<RecipeServiceModel>, IHaveCustomMappings
    {
        public RecipeCreateInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        [Display(Name = "Title")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.RecipeTitleMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.RecipeTitleMinLength)]
        public string Title { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public IFormFile Photo { get; set; }

        [Display(Name = "Category Title")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.CategoryTitleMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.CategoryTitleMinLength)]
        public string CategoryTitle { get; set; }

        [Display(Name = "Summary")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.RecipeSummaryMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.RecipeSummaryMinLength)]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(Name = "Directions")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Directions { get; set; }

        [Display(Name = "Shopping List")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public RecipeCreateShoppingListInputModel ShoppingList { get; set; }

        [Display(Name = "Preparation Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.RecipeTimeMinValue, AttributesConstraints.RecipeTimeMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int PreparationTime { get; set; }

        [Display(Name = "Cooking Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(AttributesConstraints.RecipeTimeMinValue, AttributesConstraints.RecipeTimeMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int CookingTime { get; set; }

        [Display(Name = "Needed Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string NeededTime { get; set; }

        [Display(Name = "Skill Level")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string SkillLevel { get; set; }

        [Display(Name = "Serving")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string Serving { get; set; }

        [Display(Name = "Yield")]
        [Range(AttributesConstraints.RecipeYieldMinValue, AttributesConstraints.RecipeYieldMaxValue, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal? Yield { get; set; }

        [Display(Name = "Nutritional Value")]
        public RecipeCreateNutritionalValueInputModel NutritionalValue { get; set; }

        [Display(Name = "Allergens")]
        public IEnumerable<string> AllergenNames { get; set; }

        [Display(Name = "Lifestyle")]
        [EnsureMinimumElements(1, ErrorMessage = AttributesErrorMessages.EnsureMinimumElementsErrorMessage)]
        public IEnumerable<string> LifestyleTypes { get; set; }

        public RecipeViewDataModel RecipeViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeCreateInputModel, RecipeServiceModel>()
                .ForMember(
                    destination => destination.Category,
                    opts => opts.MapFrom(origin => new CategoryServiceModel { Title = origin.CategoryTitle }))
                .ForMember(
                    destination => destination.NeededTime,
                    opts => opts.Ignore());
        }
    }
}
