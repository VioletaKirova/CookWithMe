namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Categories;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class CategoryServiceTests
    {
        [Fact]
        public async Task CreateAllAsync_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { "Salads", "Main Dishes" };

            // Act
            var result = await categoryService.CreateAllAsync(categoryTitles);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAllAsync_WithDummyData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { "Salads", "Main Dishes" };

            // Act
            await categoryService.CreateAllAsync(categoryTitles);
            var actualResult = await categoryRepository.All().Select(x => x.Title).ToListAsync();
            var expectedResult = categoryTitles;

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i] == actualResult[i], errorMessagePrefix + " " + "Expected title and actual title do not match.");
            }
        }

        [Fact]
        public async Task CreateAllAsync_WithZeroData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService CreateAllAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryTitles = new string[] { };

            // Act
            var result = await categoryService.CreateAllAsync(categoryTitles);

            // Assert
            Assert.False(result, errorMessagePrefix + " " + "Returns true.");
        }

        [Fact]
        public async Task GetAllTitlesAsync_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService GetAllTitlesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
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

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var existentId = categoryRepository.All().First().Id;

            // Act
            var actualResult = await categoryService.GetByIdAsync(existentId);
            var expectedResult = (await categoryRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<CategoryServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Title == expectedResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var nonExistentId = 10;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task CreateAsync_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryServiceModel = new Category { Title = "Salads" }.To<CategoryServiceModel>();

            // Act
            var result = await categoryService.CreateAsync(categoryServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAsync_WithDummyData_ShouldSuccessfullyCreate()
        {
            string errorMessagePrefix = "CategoryService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryServiceModel = new Category { Title = "Salads" }.To<CategoryServiceModel>();

            // Act
            await categoryService.CreateAsync(categoryServiceModel);
            var actualResult = (await categoryRepository
                .All()
                .SingleOrDefaultAsync(x => x.Title == categoryServiceModel.Title))
                .To<CategoryServiceModel>();
            var expectedResult = categoryServiceModel;

            // Assert
            Assert.True(actualResult.Title == expectedResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithDummyData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryServiceModel = categoryRepository.All().First().To<CategoryServiceModel>();
            categoryServiceModel.Title = "New Title";

            // Act
            var result = await categoryService.EditAsync(categoryServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            string errorMessagePrefix = "CategoryService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryServiceModel = categoryRepository.All().First().To<CategoryServiceModel>();
            var newTitle = "New Title";
            categoryServiceModel.Title = newTitle;

            // Act
            await categoryService.EditAsync(categoryServiceModel);
            var actualResult = categoryRepository
                .All()
                .First()
                .To<CategoryServiceModel>()
                .Title;
            var expectedResult = newTitle;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Title is not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var categoryServiceModel = new CategoryServiceModel
            {
                Id = 10,
                Title = "NonExistent",
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryService.EditAsync(categoryServiceModel);
            });
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var existentId = categoryRepository.All().First().Id;

            // Act
            var result = await categoryService.DeleteByIdAsync(existentId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldSuccessfullyDelete()
        {
            string errorMessagePrefix = "CategoryService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var existentId = categoryRepository.All().First().Id;

            // Act
            var categoriesCount = categoryRepository.All().Count();
            await categoryService.DeleteByIdAsync(existentId);
            var actualResult = categoryRepository.All().Count();
            var expectedResult = categoriesCount - 1;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Categories count is not reduced.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var nonExistentId = 10;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryService.DeleteByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task SetCategoryToRecipeAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            string errorMessagePrefix = "CategoryService SetCategoryToRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var recipe = new Recipe();

            // Act
            await categoryService.SetCategoryToRecipeAsync("Salads", recipe);
            var actualResult = recipe.Category;
            var expectedResult = await categoryRepository
                .All()
                .SingleOrDefaultAsync(x => x.Title == "Salads");

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Title == expectedResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
        }

        [Fact]
        public async Task SetCategoryToRecipeAsync_WithNonExistentCategory_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var recipe = new Recipe();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryService.SetCategoryToRecipeAsync("NonExistent", recipe);
            });
        }

        [Fact]
        public async Task GetAll_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService GetAll() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);

            // Act
            var actualResult = await categoryService.GetAll().ToListAsync();
            var expectedResult = this.GetDummyData().To<CategoryServiceModel>().ToList();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(actualResult[i].Title == expectedResult[i].Title, errorMessagePrefix + " " + "Title is not returned properly.");
            }
        }

        [Fact]
        public async Task GetIdByTitleAsync_WithExistentTitle_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "CategoryService GetIdByTitleAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var existentTitle = "Salads";

            // Act
            var actualResult = await categoryService.GetIdByTitleAsync(existentTitle);
            var expectedResult = (await categoryRepository
                .All()
                .SingleOrDefaultAsync(x => x.Title == existentTitle))
                .Id;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Id is not returned properly.");
        }

        [Fact]
        public async Task GetIdByTitleAsync_WithNonExistentTitle_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var categoryRepository = new EfDeletableEntityRepository<Category>(context);
            var categoryService = new CategoryService(categoryRepository);
            var nonExistentTitle = "NonExistent";

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryService.GetIdByTitleAsync(nonExistentTitle);
            });
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
