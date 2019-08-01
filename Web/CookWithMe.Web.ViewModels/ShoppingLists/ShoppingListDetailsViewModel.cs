namespace CookWithMe.Web.ViewModels.ShoppingLists
{
    using System.Collections.Generic;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class ShoppingListDetailsViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public ICollection<string> IngredientsList { get; set; }

        public string RecipeId { get; set; }

        public string RecipeTitle { get; set; }
    }
}
