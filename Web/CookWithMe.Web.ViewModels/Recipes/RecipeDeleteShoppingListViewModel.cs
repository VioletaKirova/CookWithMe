namespace CookWithMe.Web.ViewModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeDeleteShoppingListViewModel : IMapFrom<ShoppingListServiceModel>
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
    }
}
