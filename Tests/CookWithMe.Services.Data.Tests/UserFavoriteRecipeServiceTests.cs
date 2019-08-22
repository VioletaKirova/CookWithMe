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
    using CookWithMe.Services.Models.Recipes;

    using Xunit;

    public class UserFavoriteRecipeServiceTests
    {
        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userFavoriteRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userFavoriteRecipeService
                .ContainsByUserIdAndRecipeIdAsync(nonExistentUserId, recipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithExistentUserIdAndNonExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userFavoriteRecipeService
                .ContainsByUserIdAndRecipeIdAsync(userId, nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task ContainsByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService ContainsByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userFavoriteRecipeService
                .ContainsByUserIdAndRecipeIdAsync(nonExistentUserId, nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService DeleteByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userFavoriteRecipeService
                .DeleteByUserIdAndRecipeIdAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService DeleteByUserIdAndRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var userFavoriteRecipesCount = userFavoriteRecipeRepository.All().Count();
            await userFavoriteRecipeService
                .DeleteByUserIdAndRecipeIdAsync(userId, recipeId);
            var actualResult = userFavoriteRecipeRepository.All().Count();
            var expectedResult = userFavoriteRecipesCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userFavoriteRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(nonExistentUserId, recipeId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithExistentUserIdAndNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userFavoriteRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(userId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task DeleteByUserIdAndRecipeIdAsync_WithNonExistentUserIdAndRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userFavoriteRecipeService
                    .DeleteByUserIdAndRecipeIdAsync(nonExistentUserId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var result = await userFavoriteRecipeService.DeleteByRecipeIdAsync(recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithExistentRecipeId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var recipeId = context.Recipes.First(x => x.Title == "Recipe 1").Id;

            // Act
            var userFavoriteRecipesCount = userFavoriteRecipeRepository.All().Count();
            await userFavoriteRecipeService.DeleteByRecipeIdAsync(recipeId);
            var actualResult = userFavoriteRecipeRepository.All().Count();
            var expectedResult = userFavoriteRecipesCount - 1;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeleteByRecipeIdAsync_WithNonExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService DeleteByRecipeIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var result = await userFavoriteRecipeService.DeleteByRecipeIdAsync(nonExistentRecipeId);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetRecipesByUserId_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService GetRecipesByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.FullName == "User 1").Id;

            // Act
            var actualResult = userFavoriteRecipeService
                .GetRecipesByUserId(userId)
                .ToList();
            var expectedResult = userFavoriteRecipeRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Select(x => x.Recipe)
                .To<RecipeServiceModel>()
                .ToList();

            // Assert
            Assert.True(expectedResult.Count() == actualResult.Count(), errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count(); i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetRecipesByUserId_WithNonExistentUserId_ShouldReturnEmptyCollection()
        {
            var errorMessagePrefix = "UserFavoriteRecipeService GetRecipesByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userFavoriteRecipeRepository = new EfRepository<UserFavoriteRecipe>(context);
            var userFavoriteRecipeService = new UserFavoriteRecipeService(userFavoriteRecipeRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var actualResult = userFavoriteRecipeService
                .GetRecipesByUserId(nonExistentUserId)
                .ToList()
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collection is not empty.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            var userWithTwoFavoriteRecipes = new ApplicationUser() { FullName = "User 1" };
            userWithTwoFavoriteRecipes.FavoriteRecipes.Add(new UserFavoriteRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 1" },
            });
            userWithTwoFavoriteRecipes.FavoriteRecipes.Add(new UserFavoriteRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 2" },
            });

            var userWithOneFavoriteRecipe = new ApplicationUser() { FullName = "User 2" };
            userWithOneFavoriteRecipe.FavoriteRecipes.Add(new UserFavoriteRecipe()
            {
                Recipe = new Recipe() { Title = "Recipe 3" },
            });
            await context.Users.AddAsync(userWithTwoFavoriteRecipes);
            await context.Users.AddAsync(userWithOneFavoriteRecipe);
            await context.SaveChangesAsync();
        }
    }
}
