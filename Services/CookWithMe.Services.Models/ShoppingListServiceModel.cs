namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class ShoppingListServiceModel : IMapTo<ShoppingList>
    {
        public string Id { get; set; }

        public string Ingredients { get; set; }
    }
}
