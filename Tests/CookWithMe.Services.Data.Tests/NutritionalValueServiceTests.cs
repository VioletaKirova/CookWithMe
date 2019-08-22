namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.NutritionalValues;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.NutritionalValues;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class NutritionalValueServiceTests
    {
        [Fact]
        public async Task GetIdByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "NutritionalValueService GetIdByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var recipeId = context.Recipes.First().Id;

            // Act
            var actualResult = await nutritionalValueService.GetIdByRecipeIdAsync(recipeId);
            var expectedResult = nutritionalValueRepository.All().First().Id;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Id is not returned properly.");
        }

        [Fact]
        public async Task GetIdByRecipeIdAsync_WithNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await nutritionalValueService.GetIdByRecipeIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "NutritionalValueService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var existentId = nutritionalValueRepository.All().First().Id;

            // Act
            var actualResult = await nutritionalValueService.GetByIdAsync(existentId);
            var expectedResult = (await nutritionalValueRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<NutritionalValueServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Calories == expectedResult.Calories, errorMessagePrefix + " " + "Calories are not returned properly.");
            Assert.True(actualResult.Fats == expectedResult.Fats, errorMessagePrefix + " " + "Fats are not returned properly.");
            Assert.True(actualResult.SaturatedFats == expectedResult.SaturatedFats, errorMessagePrefix + " " + "SaturatedFats are not returned properly.");
            Assert.True(actualResult.Carbohydrates == expectedResult.Carbohydrates, errorMessagePrefix + " " + "Carbohydrates are not returned properly.");
            Assert.True(actualResult.Sugar == expectedResult.Sugar, errorMessagePrefix + " " + "Sugar is not returned properly.");
            Assert.True(actualResult.Protein == expectedResult.Protein, errorMessagePrefix + " " + "Protein is not returned properly.");
            Assert.True(actualResult.Fiber == expectedResult.Fiber, errorMessagePrefix + " " + "Fiber is not returned properly.");
            Assert.True(actualResult.Salt == expectedResult.Salt, errorMessagePrefix + " " + "Salt is not returned properly.");
            Assert.True(actualResult.RecipeId == expectedResult.RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await nutritionalValueService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            var errorMessagePrefix = "NutritionalValueService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var nutritionalValueServiceModel = nutritionalValueRepository
                .All()
                .First()
                .To<NutritionalValueServiceModel>();
            var nutritionalValueId = nutritionalValueServiceModel.Id;
            var newValue = 10;
            nutritionalValueServiceModel.Calories = newValue;
            nutritionalValueServiceModel.Fats = newValue;
            nutritionalValueServiceModel.SaturatedFats = newValue;
            nutritionalValueServiceModel.Carbohydrates = newValue;
            nutritionalValueServiceModel.Sugar = newValue;
            nutritionalValueServiceModel.Protein = newValue;
            nutritionalValueServiceModel.Fiber = newValue;
            nutritionalValueServiceModel.Salt = newValue;

            // Act
            await nutritionalValueService.EditAsync(nutritionalValueId, nutritionalValueServiceModel);
            var actualResult = nutritionalValueRepository.All().First();
            var expectedValue = newValue;

            // Assert
            Assert.True(actualResult.Calories == expectedValue, errorMessagePrefix + " " + "Calories are not returned properly.");
            Assert.True(actualResult.Fats == expectedValue, errorMessagePrefix + " " + "Fats are not returned properly.");
            Assert.True(actualResult.SaturatedFats == expectedValue, errorMessagePrefix + " " + "SaturatedFats are not returned properly.");
            Assert.True(actualResult.Carbohydrates == expectedValue, errorMessagePrefix + " " + "Carbohydrates are not returned properly.");
            Assert.True(actualResult.Sugar == expectedValue, errorMessagePrefix + " " + "Sugar is not returned properly.");
            Assert.True(actualResult.Protein == expectedValue, errorMessagePrefix + " " + "Protein is not returned properly.");
            Assert.True(actualResult.Fiber == expectedValue, errorMessagePrefix + " " + "Fiber is not returned properly.");
            Assert.True(actualResult.Salt == expectedValue, errorMessagePrefix + " " + "Salt is not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var nutritionalValueServiceModel = new NutritionalValueServiceModel();
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await nutritionalValueService.EditAsync(nonExistentId, nutritionalValueServiceModel);
            });
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "NutritionalValueService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var existentId = nutritionalValueRepository.All().First().Id;

            // Act
            var result = await nutritionalValueService.DeleteByIdAsync(existentId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "NutritionalValueService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var existentId = nutritionalValueRepository.All().First().Id;

            // Act
            var nutritionalValuesCount = nutritionalValueRepository.All().Count();
            await nutritionalValueService.DeleteByIdAsync(existentId);
            var actualResult = nutritionalValueRepository.All().Count();
            var expectedResult = nutritionalValuesCount - 1;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "NutritionalValues count is not reduced.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var nutritionalValueRepository = new EfDeletableEntityRepository<NutritionalValue>(context);
            var nutritionalValueService = new NutritionalValueService(nutritionalValueRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await nutritionalValueService.DeleteByIdAsync(nonExistentId);
            });
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            var recipe = new Recipe
            {
                NutritionalValue = new NutritionalValue(),
            };

            await context.AddAsync(recipe);
            await context.SaveChangesAsync();
        }
    }
}
