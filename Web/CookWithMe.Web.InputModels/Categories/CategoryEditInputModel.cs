﻿namespace CookWithMe.Web.InputModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryEditInputModel : IMapTo<CategoryServiceModel>, IMapFrom<CategoryServiceModel>
    {
        public int Id { get; set; }

        [Display(Name = "Category Title")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [StringLength(20, ErrorMessage = ErrorMessages.StringLengthErrorMessage, MinimumLength = 3)]
        public string Title { get; set; }
    }
}