namespace CookWithMe.Web.ViewModels.Recipes
{
    using System.Collections.Generic;
    using System.Linq;

    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeDetailsViewModel : IMapFrom<RecipeServiceModel>
    {
        public RecipeDetailsViewModel()
        {
            this.Reviews = new HashSet<RecipeDetailsReviewViewModel>();
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public string Summary { get; set; }

        public string Directions { get; set; }

        public string ShoppingListId { get; set; }

        public RecipeDetailsShoppingListViewModel ShoppingList { get; set; }

        public Level SkillLevel { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        public Period NeededTime { get; set; }

        public Size Serving { get; set; }

        public string NutritionalValueId { get; set; }

        public RecipeDetailsNutritionalValueViewModel NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public string UserId { get; set; }

        public RecipeDetailsApplicationUserViewModel User { get; set; }

        public ICollection<RecipeDetailsReviewViewModel> Reviews { get; set; }

        public int Rate()
        {
            return this.Reviews != null ?
                this.Reviews.Sum(x => x.Rating) / this.Reviews.Count() :
                -1;
        }

        public string DisplayTime(int time)
        {
            int hours = time / 60;
            int minutes = time % 60;

            return hours == 0 ?
                $" {minutes} min" : minutes == 0 ?
                $" {hours} h" :
                $" {hours} h {minutes} min";
        }
    }
}
