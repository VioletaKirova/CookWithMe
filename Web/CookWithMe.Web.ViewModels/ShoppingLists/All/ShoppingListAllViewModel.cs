namespace CookWithMe.Web.ViewModels.ShoppingLists.All
{
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;
    using CookWithMe.Services.Models.ShoppingLists;

    public class ShoppingListAllViewModel : IMapFrom<ShoppingListServiceModel>
    {
        public string Id { get; set; }

        public string RecipeId { get; set; }

        public string RecipeTitle { get; set; }
    }
}
