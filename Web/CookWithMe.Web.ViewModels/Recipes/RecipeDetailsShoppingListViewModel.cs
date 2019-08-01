namespace CookWithMe.Web.ViewModels.Recipes
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeDetailsShoppingListViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public string Ingredients { get; set; }
    }
}
