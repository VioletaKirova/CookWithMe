namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Recommendations;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Web.MLModels.DataModels;

    using Microsoft.Extensions.ML;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class RecommendationServiceTests
    {
        //[Fact]
        //public async Task GetRecommendedRecipeAsync_WithCorrectData_ShouldWorkProperly()
        //{
        //    var errorMessagePrefix = "RecommendationService GetRecommendedRecipeAsync() method does not work properly.";

        //    // Arrange
        //    MapperInitializer.InitializeMapper();
        //    var context = ApplicationDbContextInMemoryFactory.InitializeContext();
        //    await this.SeedDataAsync(context);
        //    var recommendationService = this.GetRecommendationService(context);
        //    var userId = context.Users.First().Id;

        //    // Act
        //    var actualResult = await recommendationService.GetRecommendedRecipeAsync(userId);
        //    var expectedResult = context.Recipes.First().To<RecipeServiceModel>();

        //    // Assert
        //    Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
        //}

        //private async Task SeedDataAsync(ApplicationDbContext context)
        //{
        //    await context.Recipes.AddAsync(new Recipe());
        //    await context.Users.AddAsync(new ApplicationUser());

        //    await context.SaveChangesAsync();
        //}

        //private RecommendationService GetRecommendationService(ApplicationDbContext context)
        //{
        //    var predictionEnginePool = new PredictionEnginePool<UserRecipe, UserRecipeScore>(
        //        new Mock<IServiceProvider>().Object,
        //        new Mock<IOptions<MLOptions>>().Object,
        //        new Mock<IOptionsFactory<PredictionEnginePoolOptions<UserRecipe, UserRecipeScore>>>().Object);

        //    var recipeService = new Mock<IRecipeService>();
        //    recipeService
        //       .Setup(x => x.GetAllFilteredAsync(Guid.NewGuid().ToString()))
        //       .Returns((string userId) =>
        //            Task.FromResult(context.Recipes.To<RecipeServiceModel>()));
        //    recipeService
        //        .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
        //       .Returns(Task.FromResult<RecipeServiceModel>(context.Recipes.First().To<RecipeServiceModel>()));

        //    return new RecommendationService(
        //        predictionEnginePool,
        //        recipeService.Object);
        //}
    }
}
