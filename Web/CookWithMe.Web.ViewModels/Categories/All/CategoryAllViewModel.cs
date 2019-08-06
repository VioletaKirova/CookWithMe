namespace CookWithMe.Web.ViewModels.Categories.All
{
    using System.Collections.Generic;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class CategoryAllViewModel : IMapFrom<CategoryServiceModel>
    {
        public CategoryAllViewModel()
        {
            this.Recipes = new HashSet<CategoryAllRecipeViewModel>();
        }

        public string Title { get; set; }

        public IEnumerable<CategoryAllRecipeViewModel> Recipes { get; set; }
    }
}
