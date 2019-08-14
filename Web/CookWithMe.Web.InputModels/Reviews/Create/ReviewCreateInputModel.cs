﻿namespace CookWithMe.Web.InputModels.Reviews.Create
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Services.Models.Reviews;
    using CookWithMe.Services.Models.Users;

    public class ReviewCreateInputModel : IMapTo<ReviewServiceModel>
    {
        public string Id { get; set; }

        public string RecipeTitle { get; set; }

        [Display(Name = "Comment")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [StringLength(250, ErrorMessage = AttributesErrorMessages.StringLengthErrorMessage, MinimumLength = 1)]
        public string Comment { get; set; }

        [Display(Name = "Rating")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [Range(1, 5,  ErrorMessage = AttributesErrorMessages.RangeErrorMessage)]
        public int Rating { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public string ReviewerId { get; set; }

        public ApplicationUserServiceModel Reviewer { get; set; }
    }
}