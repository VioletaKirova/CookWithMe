namespace CookWithMe.Web.InputModels.Recipes
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeCreateShoppingListInputModel : IMapTo<ShoppingListServiceModel>
    {
        public string Ingredients { get; set; }
    }
}
