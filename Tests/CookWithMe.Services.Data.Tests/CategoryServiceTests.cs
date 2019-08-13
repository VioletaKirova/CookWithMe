namespace CookWithMe.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Tests.Common;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class CategoryServiceTests
    {
        [Fact]
        public async Task CreateAllAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { "Salads", "Main Dishes" };

            // Act
            var actualResult = await categoryService.CreateAllAsync(categoryTitles);

            // Assert
            Assert.True(actualResult, errorMessagePrefix + " " + "Result > 0 is false.");
        }

        [Fact]
        public async Task CreateAllAsync_WithCorrectData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { "Salads", "Main Dishes" };

            // Act
            await categoryService.CreateAllAsync(categoryTitles);
            var actualData = await categoryRepository.AllAsNoTracking().Select(x => x.Title).ToListAsync();
            var expectedData = this.GetDummyData().Select(x => x.Title).ToList();


            // Assert
            for (int i = 0; i < actualData.Count; i++)
            {
                Assert.True(expectedData[i] == actualData[i], errorMessagePrefix + " " + "Expected title and actual title do not match.");
            }
        }

        [Fact]
        public async Task CreateAllAsync_WithZeroData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { };

            // Act
            var actualResult = await categoryService.CreateAllAsync(categoryTitles);

            // Assert
            Assert.False(actualResult, errorMessagePrefix + " " + "Result > 0 is true.");
        }

        [Fact]
        public async Task GetAllTitlesAsync_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService GetAllTitlesAsync() method does not work properly.";

            // Arrange
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);

            // Act
            var actualResult = (await categoryService.GetAllTitlesAsync()).ToList();
            var expectedResult = this.GetDummyData().Select(x => x.Title).ToList();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected title and actual title do not match.");
            }
        }

        private List<Category> GetDummyData()
        {
            return new List<Category>()
            {
                new Category() { Title = "Salads" },
                new Category() { Title = "Main Dishes" },
            };
        }

        private async Task SeedData(ApplicationDbContext context)
        {
            context.AddRange(this.GetDummyData());
            await context.SaveChangesAsync();
        }
    }
}
