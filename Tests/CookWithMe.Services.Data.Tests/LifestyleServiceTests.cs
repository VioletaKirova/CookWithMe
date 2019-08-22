namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Lifestyles;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class LifestyleServiceTests
    {
        [Fact]
        public async Task CreateAllAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "LifestyleService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var lifestyleTypes = new string[] { "Vegetarian", "Dairy Free" };

            // Act
            var result = await lifestyleService.CreateAllAsync(lifestyleTypes);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAllAsync_WithCorrectData_ShouldSuccessfullyCreate()
        {
            var errorMessagePrefix = "LifestyleService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var lifestyleTypes = new string[] { "Vegetarian", "Dairy Free" };

            // Act
            await lifestyleService.CreateAllAsync(lifestyleTypes);
            var actualResult = await lifestyleRepository
                .All()
                .Select(x => x.Type)
                .ToListAsync();
            var expectedResult = lifestyleTypes;

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected type and actual type do not match.");
            }
        }

        [Fact]
        public async Task CreateAllAsync_WithZeroData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "LifestyleService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var lifestyleTypes = new string[] { };

            // Act
            var result = await lifestyleService.CreateAllAsync(lifestyleTypes);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetAllTypesAsync_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "LifestyleService GetAllTypesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);

            // Act
            var actualResult = (await lifestyleService.GetAllTypesAsync()).ToList();
            var expectedResult = this.GetDummyData().Select(x => x.Type).ToList();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected type and actual type do not match.");
            }
        }

        [Fact]
        public async Task SetLifestyleToRecipeAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "LifestyleService SetLifestyleToRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var recipe = new Recipe();

            // Act
            await lifestyleService.SetLifestyleToRecipeAsync("Vegetarian", recipe);
            var actualResult = recipe.Lifestyles.First().Lifestyle;
            var expectedResult = await lifestyleRepository
                .All()
                .SingleOrDefaultAsync(x => x.Type == "Vegetarian");

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Type == expectedResult.Type, errorMessagePrefix + " " + "Type is not returned properly.");
        }

        [Fact]
        public async Task SetLifestyleToRecipeAsync_WithNonExistentLifestyle_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var recipe = new Recipe();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await lifestyleService.SetLifestyleToRecipeAsync("NonExistent", recipe);
            });
        }

        [Fact]
        public async Task SetLifestyleToUserAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "LifestyleService SetLifestyleToUserAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var user = new ApplicationUser();

            // Act
            await lifestyleService.SetLifestyleToUserAsync("Vegetarian", user);
            var actualResult = user.Lifestyle;
            var expectedResult = await lifestyleRepository
                .All()
                .SingleOrDefaultAsync(x => x.Type == "Vegetarian");

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Type == expectedResult.Type, errorMessagePrefix + " " + "Type is not returned properly.");
        }

        [Fact]
        public async Task SetLifestyleToUserAsync_WithNonExistentLifestyle_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var user = new ApplicationUser();

            // Act

            // Arrange
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await lifestyleService.SetLifestyleToUserAsync("NonExistent", user);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "LifestyleService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var existentId = lifestyleRepository.All().First().Id;

            // Act
            var actualResult = await lifestyleService.GetByIdAsync(existentId);
            var expectedResult = (await lifestyleRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<LifestyleServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Type == expectedResult.Type, errorMessagePrefix + " " + "Type is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var nonExistentId = 10000;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await lifestyleService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetIdByTypeAsync_WithExistentType_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "LifestyleService GetIdByTypeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var existentType = "Vegetarian";

            // Act
            var actualResult = await lifestyleService.GetIdByTypeAsync(existentType);
            var expectedResult = (await lifestyleRepository
                .All()
                .SingleOrDefaultAsync(x => x.Type == existentType))
                .Id;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Id is not returned properly.");
        }

        [Fact]
        public async Task GetIdByTypeAsync_WithNonExistentType_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var lifestyleRepository = new EfRepository<Lifestyle>(context);
            var lifestyleService = new LifestyleService(lifestyleRepository);
            var nonExistentType = "NonExistent";

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await lifestyleService.GetIdByTypeAsync(nonExistentType);
            });
        }

        private List<Lifestyle> GetDummyData()
        {
            return new List<Lifestyle>()
            {
                new Lifestyle() { Type = "Vegetarian" },
                new Lifestyle() { Type = "Dairy Free" },
            };
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }
    }
}
