namespace CookWithMe.Web.ViewModels.Recommendations.Recommend
{
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class RecommendationRecommendViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public string Summary { get; set; }

        public string NeededTimeDescription { get; set; }

        public Size Serving { get; set; }
    }
}
