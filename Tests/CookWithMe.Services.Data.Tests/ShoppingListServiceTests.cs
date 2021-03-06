﻿namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class ShoppingListServiceTests
    {
        [Fact]
        public async Task GetIdByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ShoppingListService GetIdByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var recipeId = context.Recipes.First().Id;

            // Act
            var actualResult = await shoppingListService.GetIdByRecipeIdAsync(recipeId);
            var expectedResult = shoppingListRepository.All().First().Id;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Id is not returned properly.");
        }

        [Fact]
        public async Task GetIdByRecipeIdAsync_WithNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await shoppingListService.GetIdByRecipeIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ShoppingListService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var existentId = shoppingListRepository.All().First().Id;

            // Act
            var actualResult = await shoppingListService.GetByIdAsync(existentId);
            var expectedResult = (await shoppingListRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<ShoppingListServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Ingredients == expectedResult.Ingredients, errorMessagePrefix + " " + "Ingredients are not returned properly.");
            Assert.True(actualResult.RecipeId == expectedResult.RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await shoppingListService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            var errorMessagePrefix = "ShoppingListService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var shoppingListServiceModel = shoppingListRepository
                .All()
                .First()
                .To<ShoppingListServiceModel>();
            var shoppingListId = shoppingListServiceModel.Id;
            var newValue = "New Ingredients";
            shoppingListServiceModel.Ingredients = newValue;

            // Act
            await shoppingListService.EditAsync(shoppingListId, shoppingListServiceModel);
            var actualResult = shoppingListRepository.All().First();
            var expectedValue = newValue;

            // Assert
            Assert.True(actualResult.Ingredients == expectedValue, errorMessagePrefix + " " + "Ingredients are not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var shoppingListServiceModel = new ShoppingListServiceModel();
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await shoppingListService.EditAsync(nonExistentId, shoppingListServiceModel);
            });
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ShoppingListService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var existentId = shoppingListRepository.All().First().Id;

            // Act
            var result = await shoppingListService.DeleteByIdAsync(existentId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "ShoppingListService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataAsync(context);
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var existentId = shoppingListRepository.All().First().Id;

            // Act
            var shoppingListsCount = shoppingListRepository.All().Count();
            await shoppingListService.DeleteByIdAsync(existentId);
            var actualResult = shoppingListRepository.All().Count();
            var expectedResult = shoppingListsCount - 1;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "ShoppingLists count is not reduced.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var shoppingListRepository = new EfDeletableEntityRepository<ShoppingList>(context);
            var shoppingListService = new ShoppingListService(shoppingListRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await shoppingListService.DeleteByIdAsync(nonExistentId);
            });
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            for (int i = 0; i < 3; i++)
            {
                var recipe = new Recipe
                {
                    ShoppingList = new ShoppingList { Ingredients = "Ingredients" },
                };

                await context.AddAsync(recipe);
            }

            await context.SaveChangesAsync();
        }
    }
}
