namespace CookWithMe.Web.InputModels.Recipes
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeEditShoppingListInputModel : IMapTo<ShoppingListServiceModel>, IMapFrom<ShoppingListServiceModel>
    {
        [Display(Name = "Ingredients")]
        [Required(ErrorMessage = ErrorMessages.RequiredErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
    }
}
