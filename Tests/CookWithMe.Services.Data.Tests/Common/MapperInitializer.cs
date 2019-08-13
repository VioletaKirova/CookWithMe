namespace CookWithMe.Services.Data.Tests.Common
{
    using System.Reflection;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    public class MapperInitializer
    {
        public static void InitializeMapper()
        {
            AutoMapperConfig.RegisterMappings(
                typeof(RecipeServiceModel).GetTypeInfo().Assembly,
                typeof(Recipe).GetTypeInfo().Assembly);
        }
    }
}
