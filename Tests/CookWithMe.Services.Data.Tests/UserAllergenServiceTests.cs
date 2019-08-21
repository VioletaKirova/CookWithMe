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
    using CookWithMe.Services.Models.Users;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class UserAllergenServiceTests
    {
        [Fact]
        public async Task GetByUserIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "UserAllergenService GetByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userAllergenRepository = new EfRepository<UserAllergen>(context);
            var userAllergenService = new UserAllergenService(userAllergenRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.Biography == "User with milk and eggs allergies").Id;

            // Act
            var actualResult = (await userAllergenService
                .GetByUserIdAsync(userId))
                .ToList();
            var expectedResult = await context.UserAllergens
                .Where(x => x.UserId == userId)
                .To<UserAllergenServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].UserId == actualResult[i].UserId, errorMessagePrefix + " " + "UserId is not returned properly.");
                Assert.True(expectedResult[i].User.Biography == actualResult[i].User.Biography, errorMessagePrefix + " " + "User Biography is not returned properly.");
                Assert.True(expectedResult[i].AllergenId == actualResult[i].AllergenId, errorMessagePrefix + " " + "AllergenId is not returned properly.");
                Assert.True(expectedResult[i].Allergen.Name == actualResult[i].Allergen.Name, errorMessagePrefix + " " + "Allergen Name is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByUserIdAsync_WithNonExistentUserId_ShouldReturnEmptyCollection()
        {
            string errorMessagePrefix = "UserAllergenService GetByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userAllergenRepository = new EfRepository<UserAllergen>(context);
            var userAllergenService = new UserAllergenService(userAllergenRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var actualResult = (await userAllergenService.GetByUserIdAsync(nonExistentUserId)).Count;
            var expectedResult = 0;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Collections is not empty.");
        }

        [Fact]
        public async Task DeletePreviousUserAllergensByUserId_WithExistentUserId_ShouldSuccessfullyDelete()
        {
            string errorMessagePrefix = "UserAllergenService DeletePreviousUserAllergensByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userAllergenRepository = new EfRepository<UserAllergen>(context);
            var userAllergenService = new UserAllergenService(userAllergenRepository);
            await this.SeedDataAsync(context);
            var userId = context.Users.First(x => x.Biography == "User with milk and eggs allergies").Id;

            // Act
            var userAllergensCount = userAllergenRepository.All().Count();
            userAllergenService.DeletePreviousUserAllergensByUserId(userId);
            await userAllergenRepository.SaveChangesAsync();
            var actualResult = userAllergenRepository.All().Count();
            var expectedResult = userAllergensCount - 2;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count is not reduced properly.");
        }

        [Fact]
        public async Task DeletePreviousUserAllergensByUserId_WithNonExistentUserId_ShouldWorkProperly()
        {
            string errorMessagePrefix = "RecipeAllergenService DeletePreviousRecipeAllergensByRecipeId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userAllergenRepository = new EfRepository<UserAllergen>(context);
            var userAllergenService = new UserAllergenService(userAllergenRepository);
            await this.SeedDataAsync(context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act
            var userAllergensCount = userAllergenRepository.All().Count();
            userAllergenService.DeletePreviousUserAllergensByUserId(nonExistentUserId);
            await userAllergenRepository.SaveChangesAsync();
            var actualResult = userAllergenRepository.All().Count();
            var expectedResult = userAllergensCount;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Count does not match.");
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            await context.Allergens.AddAsync(new Allergen { Name = "Milk" });
            await context.Allergens.AddAsync(new Allergen { Name = "Eggs" });
            await context.SaveChangesAsync();

            var userWithMilkAndEggsAllergies = new ApplicationUser() { Biography = "User with milk and eggs allergies" };
            userWithMilkAndEggsAllergies.Allergies
                .Add(new UserAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Milk"),
                });
            userWithMilkAndEggsAllergies.Allergies
                .Add(new UserAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Eggs"),
                });
            await context.Users.AddAsync(userWithMilkAndEggsAllergies);

            var userWithMilkAllergy = new ApplicationUser() { Biography = "User with milk allergy" };
            userWithMilkAllergy.Allergies
                .Add(new UserAllergen
                {
                    Allergen = context.Allergens.First(x => x.Name == "Milk"),
                });
            await context.Users.AddAsync(userWithMilkAllergy);

            await context.SaveChangesAsync();
        }
    }
}
