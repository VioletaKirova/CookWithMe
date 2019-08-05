namespace CookWithMe.Web.InputModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeEditShoppingListInputModel : IMapTo<ShoppingListServiceModel>, IMapFrom<ShoppingListServiceModel>
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
    }
}
