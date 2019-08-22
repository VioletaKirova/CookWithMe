namespace CookWithMe.Web.ViewModels.Recipes.Delete
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.ViewModels.Recipes.ViewData;

    using Microsoft.AspNetCore.Http;

    public class RecipeDeleteViewModel : IMapFrom<RecipeServiceModel>, IHaveCustomMappings
    {
        public RecipeDeleteViewModel()
        {
            this.AllergenNames = new HashSet<string>();
            this.LifestyleTypes = new HashSet<string>();
        }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string CategoryTitle { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Directions { get; set; }

        [Required]
        public RecipeDeleteShoppingListViewModel ShoppingList { get; set; }

        [Required]
        public int PreparationTime { get; set; }

        [Required]
        public int CookingTime { get; set; }

        [Required]
        public string NeededTime { get; set; }

        [Required]
        public string SkillLevel { get; set; }

        public IEnumerable<string> AllergenNames { get; set; }

        [Required]
        public IEnumerable<string> LifestyleTypes { get; set; }

        [Required]
        public string Serving { get; set; }

        public decimal? Yield { get; set; }

        public RecipeDeleteNutritionalValueViewModel NutritionalValue { get; set; }

        public RecipeViewDataModel RecipeViewData { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeServiceModel, RecipeDeleteViewModel>()
                .ForMember(
                    destination => destination.CategoryTitle,
                    opts => opts.MapFrom(origin => origin.Category.Title))
                .ForMember(
                    destination => destination.Photo,
                    opts => opts.Ignore());
        }
    }
}
