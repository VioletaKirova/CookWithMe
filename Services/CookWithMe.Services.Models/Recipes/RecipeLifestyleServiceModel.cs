namespace CookWithMe.Services.Models.Recipes
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Lifestyles;

    public class RecipeLifestyleServiceModel : IMapTo<RecipeLifestyle>, IMapFrom<RecipeLifestyle>
    {
        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public int LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }
    }
}
