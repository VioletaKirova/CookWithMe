namespace CookWithMe.Web.InputModels.Categories.Create
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;

    public class CategoryCreateInputModel : IMapTo<CategoryServiceModel>
    {
        [Display(Name = "Category Title")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(AttributesConstraints.CategoryTitleMaxLength, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = AttributesConstraints.CategoryTitleMinLength)]
        public string Title { get; set; }
    }
}
