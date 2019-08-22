namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class RecipeLifestyleServiceTests
    {
        [Fact]
        public async Task GetByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeLifestyleService GetByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe for vegetarians and vegans").Id;

            // Act
            var actualResult = (await recipeLifestyleService
                .GetByRecipeIdAsync(recipeId))
                .ToList();
            var expectedResult = await context.RecipeLifestyles
                .Where(x => x.RecipeId == recipeId)
                .To<RecipeLifestyleServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].RecipeId == actualResult[i].RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
                Assert.True(expectedResult[i].Recipe.Title == actualResult[i].Recipe.Title, errorMessagePrefix + " " + "Recipe Title is not returned properly.");
                Assert.True(expectedResult[i].LifestyleId == actualResult[i].LifestyleId, errorMessagePrefix + " " + "LifestyleId is not returned properly.");
                Assert.True(expectedResult[i].Lifestyle.Type == actualResult[i].Lifestyle.Type, errorMessagePrefix + " " + "Lifestyle Type is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByRecipeIdAsync_WithNonExistentRecipeId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "RecipeLifestyleService GetByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var recipeId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await recipeLifestyleService.GetByRecipeIdAsync(recipeId)).Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections is not empty.");
        }

        [Fact]
        public async Task DeletePreviousRecipeLifestylesByRecipeId_WithExistentRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "RecipeLifestyleService DeletePreviousRecipeLifestylesByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe for vegetarians and vegans").Id;

            // Act
            var recipeLifestylesCount = recipeLifestyleRepository.All().Count();
            recipeLifestyleService.DeletePreviousRecipeLifestylesByRecipeId(recipeId);
            await recipeLifestyleRepository.SaveChangesAsync();
            var actualResult = recipeLifestyleRepository.All().Count();
            var expectedResult = recipeLifestylesCount - 2;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeletePreviousRecipeLifestylesByRecipeId_WithNonExistentRecipeId_ShouldWorkProperly()
        {
            var errorMessagePrefix = "RecipeLifestyleService DeletePreviousRecipeLifestylesByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var recipeId = Guid.NewGuid().ToString();

            // Act
            var recipeAllergensCount = recipeLifestyleRepository.All().Count();
            recipeLifestyleService.DeletePreviousRecipeLifestylesByRecipeId(recipeId);
            await recipeLifestyleRepository.SaveChangesAsync();
            var actualResult = recipeLifestyleRepository.All().Count();
            var expectedResult = recipeAllergensCount;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count does not match.");
        }

        [Fact]
        public async Task GetRecipeIdsByLifestyleIdAsync_WithExistentLifestyleId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeLifestyleService GetRecipeIdsByLifestyleIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var lifestyleId = context.Lifestyles.First(x => x.Type == "Vegetarian").Id;

            // Act
            var actualResult = (await recipeLifestyleService
                .GetRecipeIdsByLifestyleIdAsync(lifestyleId))
                .ToList();
            var expectedResult = await context.RecipeLifestyles
                .Where(x => x.LifestyleId == lifestyleId)
                .Select(x => x.RecipeId)
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "RecipeId is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRecipeIdsByLifestyleIdAsync_WithNonExistentLifestyleId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "RecipeLifestyleService GetRecipeIdsByLifestyleIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeLifestyleRepository = new EfRepository<RecipeLifestyle>(context);
            var recipeLifestyleService = new RecipeLifestyleService(recipeLifestyleRepository);
            await this.SeedDataAsync(context);
            var nonExistentLifestyleId = 10000;

            // Act
            var actualResult = (await recipeLifestyleService
                .GetRecipeIdsByLifestyleIdAsync(nonExistentLifestyleId))
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections is not empty.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            await context.Lifestyles.AddAsync(new Lifestyle { Type = "Vegetarian" });
            await context.Lifestyles.AddAsync(new Lifestyle { Type = "Vegan" });
            await context.SaveChangesAsync();

            var recipeForVegetariansAndVegans = new Recipe() { Title = "Recipe for vegetarians and vegans" };
            recipeForVegetariansAndVegans.Lifestyles
                .Add(new RecipeLifestyle
                {
                    Lifestyle = context.Lifestyles.First(x => x.Type == "Vegetarian"),
                });
            recipeForVegetariansAndVegans.Lifestyles
                .Add(new RecipeLifestyle
                {
                    Lifestyle = context.Lifestyles.First(x => x.Type == "Vegan"),
                });
            await context.Recipes.AddAsync(recipeForVegetariansAndVegans);

            var recipeForVegans = new Recipe() { Title = "Recipe for vegans" };
            recipeForVegans.Lifestyles
                .Add(new RecipeLifestyle
                {
                    Lifestyle = context.Lifestyles.First(x => x.Type == "Vegan"),
                });
            await context.Recipes.AddAsync(recipeForVegans);

            await context.SaveChangesAsync();
        }
    }
}
