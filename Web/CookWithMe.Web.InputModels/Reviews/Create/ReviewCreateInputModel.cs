﻿namespace CookWithMe.Web.InputModels.Reviews.Create
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Reviews;

    public class ReviewCreateInputModel : IMapTo<ReviewServiceModel>
    {
        [Display(Name = "Comment")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(250, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 1)]
        public string Comment { get; set; }

        [Display(Name = "Rating")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(1, 5,  ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int Rating { get; set; }

        public ReviewCreateRecipeInputModel Recipe { get; set; }
    }
}
