namespace CookWithMe.Web.ViewModels.Recipes.Details
{
    using System;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Reviews;

    public class RecipeDetailsReviewViewModel : IMapFrom<ReviewServiceModel>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public string ReviewerUserName { get; set; }

        public string ReviewerFullName { get; set; }

        public string ReviewerProfilePhoto { get; set; }
    }
}
