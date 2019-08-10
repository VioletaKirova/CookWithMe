namespace CookWithMe.Web.ViewModels.ShoppingLists.Details
{
    using System.Collections.Generic;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Services.Models.ShoppingLists;

    public class ShoppingListDetailsViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public IList<string> IngredientsList { get; set; }

        public string RecipeId { get; set; }

        public string RecipeTitle { get; set; }
    }
}
