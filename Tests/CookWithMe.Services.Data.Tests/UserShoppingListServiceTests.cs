namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.ShoppingLists;

    using Xunit;

    public class UserShoppingListServiceTests
    {
        [Fact]
        public async Task ContainsByUserIdAndShoppingListIdAsync_WithExistentUserIdAndShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var result = await userShoppingListService
                .ContainsByUserIdAndShoppingListIdAsync(userId, shoppingListId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task ContainsByUserIdAndShoppingListIdAsync_WithNonExistentUserIdAndExistentShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var result = await userShoppingListService
                .ContainsByUserIdAndShoppingListIdAsync(nonExistentUserId, shoppingListId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndShoppingListIdAsync_WithExistentUserIdAndNonExistentShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentShoppingListId = Guid.NewGuid().ToString();

            // Act
            var result = await userShoppingListService
                .ContainsByUserIdAndShoppingListIdAsync(userId, nonExistentShoppingListId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndShoppingListIdAsync_WithNonExistentUserIdAndShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentShoppingListId = Guid.NewGuid().ToString();

            // Act
            var result = await userShoppingListService
                .ContainsByUserIdAndShoppingListIdAsync(nonExistentUserId, nonExistentShoppingListId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithExistentUserIdAndShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService DeleteByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var result = await userShoppingListService
                .DeleteByUserIdAndShoppingListIdAsync(userId, shoppingListId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithExistentUserIdAndShoppingListId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserShoppingListService DeleteByUserIdAndShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var userShoppingListsCount = userShoppingListRepository.All().Count();
            await userShoppingListService
                .DeleteByUserIdAndShoppingListIdAsync(userId, shoppingListId);
            var actualResult = userShoppingListRepository.All().Count();
            var expectedResult = userShoppingListsCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithNonExistentUserIdAndExistentShoppingListId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userShoppingListService
                    .DeleteByUserIdAndShoppingListIdAsync(nonExistentUserId, shoppingListId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithExistentUserIdAndNonExistentShoppingListId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentShoppingListId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userShoppingListService
                    .DeleteByUserIdAndShoppingListIdAsync(userId, nonExistentShoppingListId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithNonExistentUserIdAndShoppingListId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentShoppingListId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userShoppingListService
                    .DeleteByUserIdAndShoppingListIdAsync(nonExistentUserId, nonExistentShoppingListId);
            });
        }

        [Fact]
        public async Task DeleteByShoppingListIdAsync_WithExistentShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var result = await userShoppingListService.DeleteByShoppingListIdAsync(shoppingListId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByShoppingListIdAsync_WithExistentShoppingListId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var shoppingListId = context.ShoppingLists.First(x => x.Ingredients == "Ingredients 1").Id;

            // Act
            var userShoppingListsCount = userShoppingListRepository.All().Count();
            await userShoppingListService.DeleteByShoppingListIdAsync(shoppingListId);
            var actualResult = userShoppingListRepository.All().Count();
            var expectedResult = userShoppingListsCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByShoppingListIdAsync_WithNonExistentShoppingListId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentShoppingListId = Guid.NewGuid().ToString();

            // Act
            var result = await userShoppingListService.DeleteByShoppingListIdAsync(nonExistentShoppingListId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetShoppingListsByUserIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserShoppingListService GetShoppingListsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;

            // Act
            var actualResult = (await userShoppingListService
                .GetShoppingListsByUserIdAsync(userId))
                .ToList();
            var expectedResult = userShoppingListRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.AddedOn)
                .Select(x => x.ShoppingList)
                .To<ShoppingListServiceModel>()
                .ToList();

            // Assert
            Assert.True(expectedResult.Count() == actualResult.Count(), errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count(); i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "ShoppingList is not returned properly.");
            }
        }

        [Fact]
        public async Task GetShoppingListsByUserIdAsync_WithNonExistentUserId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "UserShoppingListService GetShoppingListsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await userShoppingListService
                .GetShoppingListsByUserIdAsync(nonExistentUserId))
                .ToList()
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collection is not empty.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            var userWithTwoShoppingLists = new ApplicationUser() { FullName = "User 1" };
            userWithTwoShoppingLists.ShoppingLists.Add(new UserShoppingList()
            {
                ShoppingList = new ShoppingList() { Ingredients = "Ingredients 1" },
            });
            userWithTwoShoppingLists.ShoppingLists.Add(new UserShoppingList()
            {
                ShoppingList = new ShoppingList() { Ingredients = "Ingredients 2" },
            });

            var userWithOneShoppingList = new ApplicationUser() { FullName = "User 2" };
            userWithOneShoppingList.ShoppingLists.Add(new UserShoppingList()
            {
                ShoppingList = new ShoppingList() { Ingredients = "Ingredients 3" },
            });
            await context.Users.AddAsync(userWithTwoShoppingLists);
            await context.Users.AddAsync(userWithOneShoppingList);
            await context.SaveChangesAsync();
        }
    }
}
