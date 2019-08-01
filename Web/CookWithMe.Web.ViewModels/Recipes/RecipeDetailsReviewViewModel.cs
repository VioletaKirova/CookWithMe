namespace CookWithMe.Web.ViewModels.Recipes
{
    using System;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeDetailsReviewViewModel : IMapFrom<ReviewServiceModel>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public string ReviewerId { get; set; }

        public string ReviewerFullName { get; set; }
    }
}
