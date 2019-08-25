namespace CookWithMe.Web.InputModels.Recipes.Edit
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

    public class RecipeEditInputModel : IMapTo<RecipeServiceModel>, IMapFrom<RecipeServiceModel>, IHaveCustomMappings
    {
        [Display(Name = "Title")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(50, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public IFormFile Photo { get; set; }

        [Display(Name = "Category Title")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string CategoryTitle { get; set; }

        [Display(Name = "Summary")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(1000, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Display(Name = "Directions")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Directions { get; set; }

        [Display(Name = "Shopping List")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public RecipeEditShoppingListInputModel ShoppingList { get; set; }

        [Display(Name = "Preparation Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(1, 300, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int PreparationTime { get; set; }

        [Display(Name = "Cooking Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(1, 300, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int CookingTime { get; set; }

        [Display(Name = "Needed Time")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string NeededTime { get; set; }

        [Display(Name = "Skill Level")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string SkillLevel { get; set; }

        [Display(Name = "Allergens")]
        public IEnumerable<string> AllergenNames { get; set; }

        [Display(Name = "Lifestyle")]
        [EnsureMinimumElements(1, ErrorMessage = AttributesErrorMessages.EnsureMinimumElementsErrorMessage)]
        public IEnumerable<string> LifestyleTypes { get; set; }

        [Display(Name = "Serving")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        public string Serving { get; set; }

        [Display(Name = "Yield")]
        [Range(0.1, 10000, ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public decimal? Yield { get; set; }

        [Display(Name = "Nutritional Value")]
        public RecipeEditNutritionalValueInputModel NutritionalValue { get; set; }

        public RecipeViewDataModel RecipeViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeEditInputModel, RecipeServiceModel>()
                .ForMember(
                    destination => destination.Category,
                    opts => opts.MapFrom(origin => new CategoryServiceModel { Title = origin.CategoryTitle }))
                .ForMember(
                    destination => destination.NeededTime,
                    opts => opts.Ignore());

            configuration.CreateMap<RecipeServiceModel, RecipeEditInputModel>()
                .ForMember(
                    destination => destination.CategoryTitle,
                    opts => opts.MapFrom(origin => origin.Category.Title))
                .ForMember(
                    destination => destination.Photo,
                    opts => opts.Ignore());
        }
    }
}
