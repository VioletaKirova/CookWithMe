namespace CookWithMe.Web.ViewModels.ShoppingLists
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class ShoppingListAllViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public string RecipeId { get; set; }

        public string RecipeTitle { get; set; }
    }
}
