namespace CookWithMe.Web.ViewModels.Home.Index
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeHomeViewModel : IMapFrom<RecipeServiceModel>
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        public string FormatedPreparationAndCookingTime { get; set; }
    }
}
