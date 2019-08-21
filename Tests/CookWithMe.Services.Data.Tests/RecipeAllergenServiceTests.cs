namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
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

    public class RecipeAllergenServiceTests
    {
        [Fact]
        public async Task GetByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeAllergenService GetByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe with milk and eggs").Id;

            // Act
            var actualResult = (await recipeAllergenService
                .GetByRecipeIdAsync(recipeId))
                .ToList();
            var expectedResult = await context.RecipeAllergens
                .Where(x => x.RecipeId == recipeId)
                .To<RecipeAllergenServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].RecipeId == actualResult[i].RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
                Assert.True(expectedResult[i].Recipe.Title == actualResult[i].Recipe.Title, errorMessagePrefix + " " + "Recipe Title is not returned properly.");
                Assert.True(expectedResult[i].AllergenId == actualResult[i].AllergenId, errorMessagePrefix + " " + "AllergenId is not returned properly.");
                Assert.True(expectedResult[i].Allergen.Name == actualResult[i].Allergen.Name, errorMessagePrefix + " " + "Allergen Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByRecipeIdAsync_WithNonExistentRecipeId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "RecipeAllergenService GetByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await recipeAllergenService.GetByRecipeIdAsync(nonExistentRecipeId)).Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections is not empty.");
        }

        [Fact]
        public async Task DeletePreviousRecipeAllergensByRecipeId_WithExistentRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "RecipeAllergenService DeletePreviousRecipeAllergensByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe with milk and eggs").Id;

            // Act
            var recipeAllergensCount = recipeAllergenRepository.All().Count();
            recipeAllergenService.DeletePreviousRecipeAllergensByRecipeId(recipeId);
            await recipeAllergenRepository.SaveChangesAsync();
            var actualResult = recipeAllergenRepository.All().Count();
            var expectedResult = recipeAllergensCount - 2;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeletePreviousRecipeAllergensByRecipeId_WithNonExistentRecipeId_ShouldWorkProperly()
        {
            var errorMessagePrefix = "RecipeAllergenService DeletePreviousRecipeAllergensByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var recipeAllergensCount = recipeAllergenRepository.All().Count();
            recipeAllergenService.DeletePreviousRecipeAllergensByRecipeId(nonExistentRecipeId);
            await recipeAllergenRepository.SaveChangesAsync();
            var actualResult = recipeAllergenRepository.All().Count();
            var expectedResult = recipeAllergensCount;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count does not match.");
        }

        [Fact]
        public async Task GetRecipeIdsByAllergenIdsAsync_WithExistentAllergenIds_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeAllergenService GetRecipeIdsByAllergenIdsAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var allergenIds = await context.Allergens.Select(x => x.Id).ToListAsync();

            // Act
            var actualResult = (await recipeAllergenService
                .GetRecipeIdsByAllergenIdsAsync(allergenIds))
                .ToList();
            var expectedResult = await context.RecipeAllergens
                .Where(x => allergenIds.Contains(x.AllergenId))
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
        public async Task GetRecipeIdsByAllergenIdsAsync_WithNonExistentAllergenIds_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "RecipeAllergenService GetRecipeIdsByAllergenIdsAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeAllergenRepository = new EfRepository<RecipeAllergen>(context);
            var recipeAllergenService = new RecipeAllergenService(recipeAllergenRepository);
            await this.SeedDataAsync(context);
            var nonExistentAllergenIds = new List<int>() { 100, 101 };

            // Act
            var actualResult = (await recipeAllergenService
                .GetRecipeIdsByAllergenIdsAsync(nonExistentAllergenIds))
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections is not empty.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            await context.Allergens.AddAsync(new Allergen { Name = "Milk" });
            await context.Allergens.AddAsync(new Allergen { Name = "Eggs" });
            await context.SaveChangesAsync();

            var recipeWithMilkAndEggs = new Recipe() { Title = "Recipe with milk and eggs" };
            recipeWithMilkAndEggs.Allergens
                .Add(new RecipeAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Milk"),
                });
            recipeWithMilkAndEggs.Allergens
                .Add(new RecipeAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Eggs"),
                });
            await context.Recipes.AddAsync(recipeWithMilkAndEggs);

            var recipeWithEggs = new Recipe() { Title = "Recipe with eggs" };
            recipeWithEggs.Allergens
                .Add(new RecipeAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Eggs"),
                });
            await context.Recipes.AddAsync(recipeWithEggs);

            await context.SaveChangesAsync();
        }
    }
}
