namespace CookWithMe.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserShoppingList
    {
        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }
    }
}
