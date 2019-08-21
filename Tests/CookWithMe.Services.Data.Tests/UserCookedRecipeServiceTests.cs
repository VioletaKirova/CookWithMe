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

    public class UserCookedRecipeServiceTests
    {
        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userCookedRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userCookedRecipeService
                .ContainsByUserIdAndRecipeIdAsync(nonExistentUserId, recipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithExistentUserIdAndNonExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userCookedRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userCookedRecipeService
                .ContainsByUserIdAndRecipeIdAsync(nonExistentUserId, nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService DeleteByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userCookedRecipeService
                .DeleteByUserIdAndRecipeIdAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserCookedRecipeService DeleteByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var userCookedRecipesCount = userCookedRecipeRepository.All().Count();
            await userCookedRecipeService
                .DeleteByUserIdAndRecipeIdAsync(userId, recipeId);
            var actualResult = userCookedRecipeRepository.All().Count();
            var expectedResult = userCookedRecipesCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userCookedRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(nonExistentUserId, recipeId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userCookedRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(userId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userCookedRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(nonExistentUserId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userCookedRecipeService.DeleteByRecipeIdAsync(recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithExistentRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserCookedRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var userCookedRecipesCount = userCookedRecipeRepository.All().Count();
            await userCookedRecipeService.DeleteByRecipeIdAsync(recipeId);
            var actualResult = userCookedRecipeRepository.All().Count();
            var expectedResult = userCookedRecipesCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithNonExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userCookedRecipeService.DeleteByRecipeIdAsync(nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetRecipeIdsByUserIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserCookedRecipeService GetRecipeIdsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;

            // Act
            var actualResult = (await userCookedRecipeService
                .GetRecipeIdsByUserIdAsync(userId))
                .ToList();
            var expectedResult = userCookedRecipeRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Select(x => x.RecipeId)
                .ToList();

            // Assert
            Assert.True(expectedResult.Count() == actualResult.Count(), errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count(); i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "RecipeId is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRecipeIdsByUserIdAsync_WithNonExistentUserId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "UserCookedRecipeService GetRecipeIdsByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userCookedRecipeRepository = new EfRepository<UserCookedRecipe>(context);
            var userCookedRecipeService = new UserCookedRecipeService(userCookedRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await userCookedRecipeService
                .GetRecipeIdsByUserIdAsync(nonExistentUserId))
                .ToList()
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collection is not empty.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            var userWithTwoCookedRecipes = new ApplicationUser() { FullName = "User 1" };
            userWithTwoCookedRecipes.CookedRecipes.Add(new UserCookedRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 1" },
            });
            userWithTwoCookedRecipes.CookedRecipes.Add(new UserCookedRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 2" },
            });

            var userWithOneCookedRecipe = new ApplicationUser() { FullName = "User 2" };
            userWithOneCookedRecipe.CookedRecipes.Add(new UserCookedRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 3" },
            });
            await context.Users.AddAsync(userWithTwoCookedRecipes);
            await context.Users.AddAsync(userWithOneCookedRecipe);
            await context.SaveChangesAsync();
        }
    }
}
