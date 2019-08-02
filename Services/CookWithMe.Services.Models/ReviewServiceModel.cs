namespace CookWithMe.Services.Models
{
    using System;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class ReviewServiceModel : IMapTo<Review>, IMapFrom<Review>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public string ReviewerId { get; set; }

        public ApplicationUserServiceModel Reviewer { get; set; }
    }
}
