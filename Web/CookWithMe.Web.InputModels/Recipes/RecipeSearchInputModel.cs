namespace CookWithMe.Web.InputModels.Recipes
{
    using System.Collections.Generic;

    using AutoMapper;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    public class RecipeSearchInputModel : IMapTo<RecipeSearchServiceModel>, IHaveCustomMappings
    {
        public RecipeSearchInputModel()
        {
            this.AllergenNames = new HashSet<string>();
        }

        public string CategoryTitle { get; set; }

        public string LifestyleType { get; set; }

        public string KeyWords { get; set; }

        public string SkillLevel { get; set; }

        public string Serving { get; set; }

        public string NeededTime { get; set; }

        public RecipeSearchNutritionalValueInputModel NutritionalValue { get; set; }

        public decimal? Yield { get; set; }

        public IEnumerable<string> AllergenNames { get; set; }


        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<RecipeSearchInputModel, RecipeSearchServiceModel>()
                .ForMember(
                    destination => destination.Category,
                    opts => opts.MapFrom(origin => new CategoryServiceModel { Title = origin.CategoryTitle }))
                .ForMember(
                    destination => destination.Lifestyle,
                    opts => opts.MapFrom(origin => new LifestyleServiceModel { Type = origin.LifestyleType }))
                .ForMember(
                    destination => destination.NeededTime,
                    opts => opts.Ignore());
        }
    }
}
