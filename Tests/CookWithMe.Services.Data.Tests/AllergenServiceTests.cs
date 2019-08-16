namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Tests.Common;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AllergenServiceTests
    {
        public AllergenServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateAllAsync_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AllergenService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var allergenNames = new string[] { "Milk", "Eggs" };

            // Act
            var result = await allergenService.CreateAllAsync(allergenNames);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAllAsync_WithDummyData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "AllergenService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var allergenNames = new string[] { "Milk", "Eggs" };

            // Act
            await allergenService.CreateAllAsync(allergenNames);
            var actualResult = await allergenRepository
                .All()
                .Select(x => x.Name)
                .ToListAsync();
            var expectedResult = allergenNames;

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected name and actual name do not match.");
            }
        }

        [Fact]
        public async Task CreateAllAsync_WithZeroData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AllergenService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var allergenNames = new string[] { };

            // Act
            var result = await allergenService.CreateAllAsync(allergenNames);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetAllNamesAsync_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AllergenService GetAllNamesAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);

            // Act
            var actualResult = (await allergenService.GetAllNamesAsync()).ToList();
            var expectedResult = this.GetDummyData().Select(x => x.Name).ToList();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected name and actual name do not match.");
            }
        }

        [Fact]
        public async Task SetAllergenToRecipeAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            string errorMessagePrefix = "AllergenService SetAllergenToRecipeAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var recipe = new Recipe();

            // Act
            await allergenService.SetAllergenToRecipeAsync("Milk", recipe);
            var actualResult = recipe.Allergens.First().Allergen;
            var expectedResult = await allergenRepository
                .All()
                .SingleOrDefaultAsync(x => x.Name == "Milk");

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Name == expectedResult.Name, errorMessagePrefix + " " + "Name is not returned properly.");
        }

        [Fact]
        public async Task SetAllergenToRecipeAsync_WithNonExistentAllergen_ShouldThrowArgumentNullException()
        {
            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var recipe = new Recipe();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await allergenService.SetAllergenToRecipeAsync("NonExistent", recipe);
            });
        }

        [Fact]
        public async Task SetAllergenToUserAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            string errorMessagePrefix = "AllergenService SetAllergenToUserAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var user = new ApplicationUser();

            // Act
            await allergenService.SetAllergenToUserAsync("Milk", user);
            var actualResult = user.Allergies.First().Allergen;
            var expectedResult = await allergenRepository
                .All()
                .SingleOrDefaultAsync(x => x.Name == "Milk");

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Name == expectedResult.Name, errorMessagePrefix + " " + "Name is not returned properly.");
        }

        [Fact]
        public async Task SetAllergenToUserAsync_WithNonExistentAllergen_ShouldThrowArgumentNullException()
        {
            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var user = new ApplicationUser();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await allergenService.SetAllergenToUserAsync("NonExistent", user);
            });
        }

        [Fact]
        public async Task GetIdsByNamesAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "AllergenService GetIdsByNamesAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var existentNames = new string[] { "Milk", "Eggs" };

            // Act
            var actualResult = (await allergenService.GetIdsByNamesAsync(existentNames)).ToList();
            var expectedResult = await allergenRepository
                .All()
                .Where(x => existentNames.Contains(x.Name))
                .Select(x => x.Id)
                .ToListAsync();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(actualResult[i] == expectedResult[i], errorMessagePrefix + " " + "Id is not returned properly.");
            }
        }

        [Fact]
        public async Task GetIdsByNamesAsync_WithNonExistentName_ShouldThrowArgumentNullException()
        {
            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var allergenRepository = new EfRepository<Allergen>(context);
            var allergenService = new AllergenService(allergenRepository);
            var existentAndNonExistentNames = new string[] { "Milk", "NonExistent" };

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await allergenService.GetIdsByNamesAsync(existentAndNonExistentNames);
            });
        }

        private List<Allergen> GetDummyData()
        {
            return new List<Allergen>()
            {
                new Allergen() { Name = "Milk" },
                new Allergen() { Name = "Eggs" },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }
    }
}
