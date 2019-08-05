namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class RecipeLifestyleServiceModel : IMapTo<RecipeLifestyle>, IMapFrom<RecipeLifestyle>
    {
        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }

        public int LifestyleId { get; set; }

        public LifestyleServiceModel Lifestyle { get; set; }
    }
}
