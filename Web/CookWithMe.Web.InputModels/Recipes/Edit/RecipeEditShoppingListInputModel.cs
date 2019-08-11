namespace CookWithMe.Web.InputModels.Recipes.Edit
{
    using System.ComponentModel.DataAnnotations;

    using CookWithMe.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    public class RecipeEditShoppingListInputModel : IMapTo<ShoppingListServiceModel>, IMapFrom<ShoppingListServiceModel>
    {
        [Display(Name = "Ingredients")]
        [Required(ErrorMessage = AttributesErrorMessages.RequiredErrorMessage)]
        [DataType(DataType.MultilineText)]
        public string Ingredients { get; set; }
    }
}
