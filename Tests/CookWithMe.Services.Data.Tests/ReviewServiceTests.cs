namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.Reviews;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Services.Models.Reviews;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    public class ReviewServiceTests
    {
        [Fact]
        public async Task CreateAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = new ReviewServiceModel
            {
                Comment = "Comment",
                Rating = 5,
                Recipe = context.Recipes.First().To<RecipeServiceModel>(),
                ReviewerId = context.Recipes.First().Id,
            };

            // Act
            var result = await reviewService.CreateAsync(reviewServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAsync_WithCorrectData_ShouldSuccessfullyCreate()
        {
            var errorMessagePrefix = "ReviewService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = new ReviewServiceModel
            {
                Comment = "Comment",
                Rating = 5,
                Recipe = context.Recipes.First().To<RecipeServiceModel>(),
                ReviewerId = context.Recipes.First().Id,
            };

            // Act
            await reviewService.CreateAsync(reviewServiceModel);
            var actualResult = reviewRepository.All().First().To<ReviewServiceModel>();
            var expectedResult = reviewServiceModel;

            // Assert
            Assert.True(expectedResult.Comment == actualResult.Comment, errorMessagePrefix + " " + "Comment is not returned properly.");
            Assert.True(expectedResult.Rating == actualResult.Rating, errorMessagePrefix + " " + "Rating is not returned properly.");
            Assert.True(expectedResult.Recipe.Id == actualResult.Recipe.Id, errorMessagePrefix + " " + "Recipe Id is not returned properly.");
            Assert.True(expectedResult.ReviewerId == actualResult.ReviewerId, errorMessagePrefix + " " + "ReviewerId is not returned properly.");
        }

        [Fact]
        public async Task CreateAsync_WithIncorrectData_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = new ReviewServiceModel
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = null,
                ReviewerId = null,
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await reviewService.CreateAsync(reviewServiceModel);
            });
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedReviewAsync(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = reviewRepository.All().First().To<ReviewServiceModel>();
            reviewServiceModel.Rating = 1;
            reviewServiceModel.Comment = "New Comment";

            // Act
            var result = await reviewService.EditAsync(reviewServiceModel.Id, reviewServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            var errorMessagePrefix = "ReviewService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedReviewAsync(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = reviewRepository.All().First().To<ReviewServiceModel>();
            reviewServiceModel.Rating = 1;
            reviewServiceModel.Comment = "New Comment";

            // Act
            await reviewService.EditAsync(reviewServiceModel.Id, reviewServiceModel);
            var actualResult = reviewRepository
                .All()
                .First(x => x.Id == reviewServiceModel.Id)
                .To<ReviewServiceModel>();
            var expectedResult = reviewServiceModel;

            // Assert
            Assert.True(expectedResult.Rating == actualResult.Rating, errorMessagePrefix + " " + "Rating is not returned properly.");
            Assert.True(expectedResult.Comment == actualResult.Comment, errorMessagePrefix + " " + "Comment is not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithNonExisterntId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedReviewAsync(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = reviewRepository.All().First().To<ReviewServiceModel>();
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await reviewService.EditAsync(nonExistentId, reviewServiceModel);
            });
        }

        [Fact]
        public async Task EditAsync_WithIncorrectProperty_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedReviewAsync(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var reviewServiceModel = reviewRepository.All().First().To<ReviewServiceModel>();
            reviewServiceModel.Rating = 0;
            reviewServiceModel.Comment = "New Comment";

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await reviewService.EditAsync(reviewServiceModel.Id, reviewServiceModel);
            });
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var review = new Review
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = context.Recipes.First().Id,
                ReviewerId = context.Recipes.First().Id,
            };
            await reviewRepository.AddAsync(review);
            await reviewRepository.SaveChangesAsync();
            var existentId = reviewRepository.All().First().Id;

            // Act
            var result = await reviewService.DeleteByIdAsync(existentId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "ReviewService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var review = new Review
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = context.Recipes.First().Id,
                ReviewerId = context.Recipes.First().Id,
            };
            await reviewRepository.AddAsync(review);
            await reviewRepository.SaveChangesAsync();
            var existentId = reviewRepository.All().First().Id;

            // Act
            var reviewsCount = reviewRepository.All().Count();
            await reviewService.DeleteByIdAsync(existentId);
            var actualResult = reviewRepository.All().Count();
            var expectedResult = reviewsCount - 1;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Reviews count is not reduced.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await reviewService.DeleteByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var review = new Review
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = context.Recipes.First().Id,
                ReviewerId = context.Recipes.First().Id,
            };
            await reviewRepository.AddAsync(review);
            await reviewRepository.SaveChangesAsync();
            var existentId = reviewRepository.All().First().Id;

            // Act
            var actualResult = await reviewService.GetByIdAsync(existentId);
            var expectedResult = (await reviewRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<ReviewServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(actualResult.Comment == expectedResult.Comment, errorMessagePrefix + " " + "Comment is not returned properly.");
            Assert.True(actualResult.Rating == expectedResult.Rating, errorMessagePrefix + " " + "Rating is not returned properly.");
            Assert.True(actualResult.RecipeId == expectedResult.RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
            Assert.True(actualResult.ReviewerId == expectedResult.ReviewerId, errorMessagePrefix + " " + "ReviewerId is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await reviewService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetByRecipeId_WithExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService GetByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var review = new Review
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = context.Recipes.First().Id,
                ReviewerId = context.Recipes.First().Id,
            };
            await reviewRepository.AddAsync(review);
            await reviewRepository.SaveChangesAsync();
            var existentRecipeId = context.Recipes.First().Id;

            // Act
            var actualResult = await reviewService
                .GetByRecipeId(existentRecipeId)
                .ToListAsync();
            var expectedResult = await reviewRepository
                .All()
                .Where(x => x.RecipeId == existentRecipeId)
                .ToListAsync();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(actualResult[i].Id == expectedResult[i].Id, errorMessagePrefix + " " + "Id is not returned properly.");
                Assert.True(actualResult[i].Comment == expectedResult[i].Comment, errorMessagePrefix + " " + "Comment is not returned properly.");
                Assert.True(actualResult[i].Rating == expectedResult[i].Rating, errorMessagePrefix + " " + "Rating is not returned properly.");
                Assert.True(actualResult[i].RecipeId == expectedResult[i].RecipeId, errorMessagePrefix + " " + "RecipeId is not returned properly.");
                Assert.True(actualResult[i].ReviewerId == expectedResult[i].ReviewerId, errorMessagePrefix + " " + "ReviewerId is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByRecipeId_WithNonExistentRecipeId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "ReviewService GetByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipeAndUser(context);
            var reviewRepository = new EfDeletableEntityRepository<Review>(context);
            var reviewService = this.GetReviewService(reviewRepository);
            var review = new Review
            {
                Comment = "Comment",
                Rating = 5,
                RecipeId = context.Recipes.First().Id,
                ReviewerId = context.Recipes.First().Id,
            };
            await reviewRepository.AddAsync(review);
            await reviewRepository.SaveChangesAsync();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await reviewService
                .GetByRecipeId(nonExistentRecipeId)
                .ToListAsync())
                .Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections count mismatch.");
        }

        private async Task SeedReviewAsync(ApplicationDbContext context)
        {
            await this.SeedRecipeAndUser(context);

            await context.Reviews.AddAsync(new Review()
            {
                Rating = 5,
                Comment = "Comment",
                Recipe = context.Recipes.First(),
                Reviewer = context.Users.First(),
            });

            await context.SaveChangesAsync();
        }

        private async Task SeedRecipeAndUser(ApplicationDbContext context)
        {
            await context.Recipes.AddAsync(new Recipe());
            await context.Users.AddAsync(new ApplicationUser());

            await context.SaveChangesAsync();
        }

        private ReviewService GetReviewService(EfDeletableEntityRepository<Review> reviewRepository)
        {
            var userServiceMock = new Mock<IUserService>();
            var recipeServiceMock = new Mock<IRecipeService>();
            var reviewService = new ReviewService(
                reviewRepository,
                userServiceMock.Object,
                recipeServiceMock.Object);

            return reviewService;
        }
    }
}
