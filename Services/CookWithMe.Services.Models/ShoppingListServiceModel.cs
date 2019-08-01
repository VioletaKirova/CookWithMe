namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class ShoppingListServiceModel : IMapTo<ShoppingList>, IMapFrom<ShoppingList>
    {
        public string Id { get; set; }

        public string Ingredients { get; set; }

        public string RecipeId { get; set; }

        public RecipeServiceModel Recipe { get; set; }
    }
}
