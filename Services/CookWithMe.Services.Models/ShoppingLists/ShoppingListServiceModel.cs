namespace CookWithMe.Services.Models.ShoppingLists
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class ShoppingListServiceModel : IMapTo<ShoppingList>, IMapFrom<ShoppingList>
    {
        public string Id { get; set; }

        public string Ingredients { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
