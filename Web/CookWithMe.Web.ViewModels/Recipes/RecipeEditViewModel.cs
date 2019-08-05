namespace CookWithMe.Web.ViewModels.Recipes
{
    using System.Collections.Generic;

    public class RecipeEditViewModel
    {
        public IEnumerable<string> CategoryTitles { get; set; }

        public IEnumerable<string> AllergenNames { get; set; }

        public IEnumerable<string> LifestyleTypes { get; set; }

        public IEnumerable<string> PeriodValues { get; set; }

        public IEnumerable<string> LevelValues { get; set; }

        public IEnumerable<string> SizeValues { get; set; }
    }
}
