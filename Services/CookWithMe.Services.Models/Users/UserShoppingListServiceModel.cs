namespace CookWithMe.Services.Models.Users
{
    using System;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    public class UserShoppingListServiceModel : IMapTo<UserShoppingList>, IMapFrom<UserShoppingList>
    {
        public string UserId { get; set; }

        public ApplicationUserServiceModel User { get; set; }

        public string ShoppingListId { get; set; }

        public ShoppingListServiceModel ShoppingList { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
