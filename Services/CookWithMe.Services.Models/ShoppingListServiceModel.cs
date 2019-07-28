namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class ShoppingListServiceModel : IMapTo<ShoppingList>
    {
        public string Ingredients { get; set; }
    }
}
