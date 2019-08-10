namespace CookWithMe.Web.ViewModels.Recipes.Delete
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    public class RecipeDeleteShoppingListViewModel : IMapFrom<ShoppingListServiceModel>
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
    }
}
