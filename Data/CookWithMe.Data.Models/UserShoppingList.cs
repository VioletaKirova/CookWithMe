namespace CookWithMe.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserShoppingList
    {
        public UserShoppingList()
        {
            this.AddedOn = DateTime.UtcNow;
        }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string ShoppingListId { get; set; }

        public ShoppingList ShoppingList { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
