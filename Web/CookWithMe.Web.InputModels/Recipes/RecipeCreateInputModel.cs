namespace CookWithMe.Web.InputModels.Recipes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.AspNetCore.Http;

    public class RecipeCreateInputModel : IMapTo<RecipeServiceModel>, IHaveCustomMappings
    {
        public RecipeCreateInputModel()
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
        public RecipeCreateShoppingListInputModel ShoppingList { get; set; }

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

        public RecipeCreateNutritionalValueInputModel NutritionalValue { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeCreateInputModel, RecipeServiceModel>()
                .ForMember(
                    destination => destination.Category,
                    opts => opts.MapFrom(origin => new CategoryServiceModel { Title = origin.CategoryTitle }))
                .ForMember(
                    destination => destination.NeededTime,
                    opts => opts.Ignore());
        }
    }
}
