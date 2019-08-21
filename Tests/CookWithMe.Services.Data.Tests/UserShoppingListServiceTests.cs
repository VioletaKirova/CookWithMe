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

    using Xunit;

    public class UserShoppingListServiceTests
    {
        [Fact]
        public async Task ContainsByUserIdAndShoppingListIdAsync_WithExistentUserIdAndShoppingListId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

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
            string errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

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
            string errorMessagePrefix = "UserShoppingListService ContainsByUserIdAndShoppingListIdAsync() method does not work properly.";

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
        public async Task DeleteByUserIdAndShoppingListIdAsync_WithExistentUserIdAndShoppingListId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserShoppingListService DeleteByUserIdAndShoppingListIdAsync() method does not work properly.";

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
            string errorMessagePrefix = "UserShoppingListService DeleteByUserIdAndShoppingListIdAsync() method does not work properly.";

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
        public async Task DeleteByShoppingListIdAsync_WithExistentShoppingListId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

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
            string errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

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
            string errorMessagePrefix = "UserShoppingListService DeleteByShoppingListIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var shoppingListId = Guid.NewGuid().ToString();

            // Act
            var result = await userShoppingListService.DeleteByShoppingListIdAsync(shoppingListId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetShoppingListIdsByUserIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserShoppingListService GetShoppingListIdsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;

            // Act
            var actualResult = (await userShoppingListService
                .GetShoppingListIdsByUserIdAsync(userId))
                .ToList();
            var expectedResult = userShoppingListRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Select(x => x.ShoppingListId)
                .ToList();

            // Assert
            Assert.True(expectedResult.Count() == actualResult.Count(), errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count(); i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "ShoppingListId is not returned properly.");
            }
        }

        [Fact]
        public async Task GetShoppingListIdsByUserIdAsync_WithNonExistentUserId_ShouldReturnEmptyCollection()
        {
            string errorMessagePrefix = "UserShoppingListService GetShoppingListIdsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userShoppingListRepository = new EfRepository<UserShoppingList>(context);
            var userShoppingListService = new UserShoppingListService(userShoppingListRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await userShoppingListService
                .GetShoppingListIdsByUserIdAsync(nonExistentUserId))
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
