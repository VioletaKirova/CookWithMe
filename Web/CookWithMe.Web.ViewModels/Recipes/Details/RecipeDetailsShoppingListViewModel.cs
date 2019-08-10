namespace CookWithMe.Web.ViewModels.Recipes.Details
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    public class RecipeDetailsShoppingListViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public string Ingredients { get; set; }
    }
}
