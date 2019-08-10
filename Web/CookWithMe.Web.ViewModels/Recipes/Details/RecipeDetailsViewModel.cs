namespace CookWithMe.Web.ViewModels.Recipes.Details
{
    using System.Collections.Generic;
    using System.Linq;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class RecipeDetailsViewModel : IMapFrom<RecipeServiceModel>
    {
        public RecipeDetailsViewModel()
        {
            this.DirectionsList = new List<string>();
            this.ShoppingListIngredients = new List<string>();
            this.Reviews = new HashSet<RecipeDetailsReviewViewModel>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public string Summary { get; set; }

        public ICollection<string> DirectionsList { get; set; }

        public string ShoppingListId { get; set; }

        public ICollection<string> ShoppingListIngredients { get; set; }

        public Level SkillLevel { get; set; }

        public string FormatedPreparationTime { get; set; }

        public string FormatedCookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        public string NutritionalValueId { get; set; }

        public RecipeDetailsNutritionalValueViewModel NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public string UserId { get; set; }

        public RecipeDetailsApplicationUserViewModel User { get; set; }

        public bool UserFavoritedCurrentRecipe { get; set; }

        public bool UserCookedCurrentRecipe { get; set; }

        public ICollection<RecipeDetailsReviewViewModel> Reviews { get; set; }

        public int Rate => this.Reviews != null && this.Reviews.Count() > 0 ?
                this.Reviews.Sum(x => x.Rating) / this.Reviews.Count() :
                -1;
    }
}
