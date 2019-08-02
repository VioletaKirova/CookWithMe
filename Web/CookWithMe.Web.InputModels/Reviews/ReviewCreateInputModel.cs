namespace CookWithMe.Web.InputModels.Reviews
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class ReviewCreateInputModel : IMapTo<ReviewServiceModel>
    {
        public string Id { get; set; }

        public string RecipeTitle { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public string ReviewerId { get; set; }

        public ApplicationUserServiceModel Reviewer { get; set; }
    }
}
