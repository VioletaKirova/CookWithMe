namespace CookWithMe.Services.Models
{
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;

    public class UserShoppingListServiceModel : IMapTo<UserShoppingList>, IMapFrom<UserShoppingList>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public int ShoppingListId { get; set; }

        public ShoppingListServiceModel ShoppingList { get; set; }
    }
}
