namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.ShoppingLists;
    using CookWithMe.Services.Models.Users;

    using Microsoft.AspNetCore.Identity;

    using Moq;

    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public async Task AddAdditionalInfoAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService AddAdditionalInfoAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataAsync(context);
            var userId = context.Users.First().Id;
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel()
            {
                Biography = "Biography",
                ProfilePhoto = "ProfilePhoto",
                Lifestyle = context.Lifestyles.First().To<LifestyleServiceModel>(),
            };
            userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel()
            {
                Allergen = context.Allergens.First().To<AllergenServiceModel>(),
            });

            // Act
            var result = await userService.AddAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task AddAdditionalInfoAsync_WithCorrectData_ShouldSuccessfullyAdd()
        {
            var errorMessagePrefix = "UserService AddAdditionalInfoAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataAsync(context);
            var userId = context.Users.First().Id;
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel()
            {
                Biography = "Biography",
                ProfilePhoto = "ProfilePhoto",
                Lifestyle = context.Lifestyles.First().To<LifestyleServiceModel>(),
            };
            userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel()
            {
                Allergen = context.Allergens.First().To<AllergenServiceModel>(),
            });

            // Act
            await userService.AddAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);
            var actualResult = userRepository.All().First(x => x.Id == userId);
            var expectedResult = userAdditionalInfoServiceModel;

            // Assert
            Assert.True(actualResult.HasAdditionalInfo, errorMessagePrefix + " " + "HasAdditionalInfo is not returned properly.");
            Assert.True(expectedResult.Biography == actualResult.Biography, errorMessagePrefix + " " + "Biography is not returned properly.");
            Assert.True(expectedResult.ProfilePhoto == actualResult.ProfilePhoto, errorMessagePrefix + " " + "ProfilePhoto is not returned properly.");
            Assert.True(expectedResult.Lifestyle.Id == actualResult.Lifestyle.Id, errorMessagePrefix + " " + "Lifestyle is not returned properly.");
            Assert.True(expectedResult.Allergies.First().Allergen.Id == actualResult.Allergies.First().Allergen.Id, errorMessagePrefix + " " + "Allergen is not returned properly.");
        }

        [Fact]
        public async Task AddAdditionalInfoAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.AddAdditionalInfoAsync(nonExistentUserId, userAdditionalInfoServiceModel);
            });
        }

        [Fact]
        public async Task EditAdditionalInfoAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService EditAdditionalInfoAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataAsync(context);
            var userId = context.Users.First().Id;
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel()
            {
                FullName = "FullName",
                Biography = "Biography",
                ProfilePhoto = "ProfilePhoto",
                Lifestyle = context.Lifestyles.First().To<LifestyleServiceModel>(),
            };
            userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel()
            {
                Allergen = context.Allergens.First().To<AllergenServiceModel>(),
            });

            // Act
            var result = await userService.EditAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task EditAdditionalInfoAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            var errorMessagePrefix = "UserService EditAdditionalInfoAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataAsync(context);
            var userId = context.Users.First().Id;
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel()
            {
                FullName = "FullName",
                Biography = "Biography",
                ProfilePhoto = "ProfilePhoto",
                Lifestyle = context.Lifestyles.First().To<LifestyleServiceModel>(),
            };
            userAdditionalInfoServiceModel.Allergies.Add(new UserAllergenServiceModel()
            {
                Allergen = context.Allergens.First().To<AllergenServiceModel>(),
            });

            // Act
            await userService.EditAdditionalInfoAsync(userId, userAdditionalInfoServiceModel);
            var actualResult = userRepository.All().First(x => x.Id == userId);
            var expectedResult = userAdditionalInfoServiceModel;

            // Assert
            Assert.True(expectedResult.FullName == actualResult.FullName, errorMessagePrefix + " " + "FullName is not returned properly.");
            Assert.True(expectedResult.Biography == actualResult.Biography, errorMessagePrefix + " " + "Biography is not returned properly.");
            Assert.True(expectedResult.ProfilePhoto == actualResult.ProfilePhoto, errorMessagePrefix + " " + "ProfilePhoto is not returned properly.");
            Assert.True(expectedResult.Lifestyle.Id == actualResult.Lifestyle.Id, errorMessagePrefix + " " + "Lifestyle is not returned properly.");
            Assert.True(expectedResult.Allergies.First().Allergen.Id == actualResult.Allergies.First().Allergen.Id, errorMessagePrefix + " " + "Allergen is not returned properly.");
        }

        [Fact]
        public async Task EditAdditionalInfoAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            var nonExistentUserId = Guid.NewGuid().ToString();
            var userAdditionalInfoServiceModel = new UserAdditionalInfoServiceModel();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.EditAdditionalInfoAsync(nonExistentUserId, userAdditionalInfoServiceModel);
            });
        }

        [Fact]
        public async Task SetUserToRecipeAsync_WithExistentUserId_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "UserService SetUserToRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var recipe = context.Recipes.First();

            // Act
            await userService.SetUserToRecipeAsync(userId, recipe);
            var actualResult = context.Recipes.First().User.Id;
            var expectedResult = userId;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "User is not returned properly.");
        }

        [Fact]
        public async Task SetUserToRecipeAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipe = context.Recipes.First();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.SetUserToRecipeAsync(nonExistentUserId, recipe);
            });
        }

        [Fact]
        public async Task SetUserToReviewAsync_WithExistentUserId_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "UserService SetUserToReviewAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Reviews.AddAsync(new Review());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var review = context.Reviews.First();

            // Act
            await userService.SetUserToReviewAsync(userId, review);
            var actualResult = context.Reviews.First().Reviewer.Id;
            var expectedResult = userId;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Reviewer is not returned properly.");
        }

        [Fact]
        public async Task SetUserToReviewAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await context.Reviews.AddAsync(new Review());
            await context.SaveChangesAsync();
            var nonExistentUserId = Guid.NewGuid().ToString();
            var review = context.Reviews.First();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.SetUserToReviewAsync(nonExistentUserId, review);
            });
        }

        [Fact]
        public async Task SetFavoriteRecipeAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService SetFavoriteRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var recipe = context.Recipes.First();

            // Act
            var result = await userService.SetFavoriteRecipeAsync(userId, recipe);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task SetFavoriteRecipeAsync_WithExistentUserId_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "UserService SetFavoriteRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var recipe = context.Recipes.First();

            // Act
            await userService.SetFavoriteRecipeAsync(userId, recipe);
            var actualResult = userRepository.All().First().FavoriteRecipes.First();
            var expectedResult = context.Recipes.First();

            // Assert
            Assert.True(expectedResult.Id == actualResult.Recipe.Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
        }

        [Fact]
        public async Task SetFavoriteRecipeAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipe = context.Recipes.First();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.SetFavoriteRecipeAsync(nonExistentUserId, recipe);
            });
        }

        [Fact]
        public async Task SetCookedRecipeAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService SetCookedRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var recipe = context.Recipes.First();

            // Act
            var result = await userService.SetCookedRecipeAsync(userId, recipe);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task SetCookedRecipeAsync_WithExistentUserId_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "UserService SetCookedRecipeAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var recipe = context.Recipes.First();

            // Act
            await userService.SetCookedRecipeAsync(userId, recipe);
            var actualResult = userRepository.All().First().CookedRecipes.First();
            var expectedResult = context.Recipes.First();

            // Assert
            Assert.True(expectedResult.Id == actualResult.Recipe.Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
        }

        [Fact]
        public async Task SetCookedRecipeAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await context.Recipes.AddAsync(new Recipe());
            await context.SaveChangesAsync();
            var nonExistentUserId = Guid.NewGuid().ToString();
            var recipe = context.Recipes.First();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.SetCookedRecipeAsync(nonExistentUserId, recipe);
            });
        }

        [Fact]
        public async Task SetShoppingListAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService SetShoppingListAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.ShoppingLists.AddAsync(new ShoppingList() { Ingredients = "Ingredients" });
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var shoppingListServiceModel = context.ShoppingLists
                .First()
                .To<ShoppingListServiceModel>();

            // Act
            var result = await userService.SetShoppingListAsync(userId, shoppingListServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task SetShoppingListAsync_WithExistentUserId_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "UserService SetShoppingListAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await userRepository.AddAsync(new ApplicationUser());
            await userRepository.SaveChangesAsync();
            await context.ShoppingLists.AddAsync(new ShoppingList() { Ingredients = "Ingredients" });
            await context.SaveChangesAsync();
            var userId = userRepository.All().First().Id;
            var shoppingListServiceModel = context.ShoppingLists
                .First()
                .To<ShoppingListServiceModel>();

            // Act
            await userService.SetShoppingListAsync(userId, shoppingListServiceModel);
            var actualResult = userRepository
                .All()
                .First()
                .ShoppingLists
                .First()
                .ShoppingList
                .To<ShoppingListServiceModel>();
            var expectedResult = shoppingListServiceModel;

            // Assert
            Assert.True(expectedResult.Id == actualResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedResult.Ingredients == actualResult.Ingredients, errorMessagePrefix + " " + "Ingredients are not returned properly.");
        }

        [Fact]
        public async Task SetShoppingListAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await context.ShoppingLists.AddAsync(new ShoppingList());
            await context.SaveChangesAsync();
            var nonExistentUserId = Guid.NewGuid().ToString();
            var shoppingListServiceModel = context.ShoppingLists
                .First()
                .To<ShoppingListServiceModel>();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.SetShoppingListAsync(nonExistentUserId, shoppingListServiceModel);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataForGetByIdMethod(context);
            var userId = userRepository.All().First().Id;

            // Act
            var actualResult = await userService.GetByIdAsync(userId);
            var expectedResult = userRepository
                .All()
                .First()
                .To<ApplicationUserServiceModel>();

            // Assert
            Assert.True(expectedResult.Id == actualResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedResult.FullName == actualResult.FullName, errorMessagePrefix + " " + "FullName is not returned properly.");
            Assert.True(expectedResult.Biography == actualResult.Biography, errorMessagePrefix + " " + "Biography is not returned properly.");
            Assert.True(expectedResult.ProfilePhoto == actualResult.ProfilePhoto, errorMessagePrefix + " " + "ProfilePhoto is not returned properly.");
            Assert.True(expectedResult.Lifestyle.Id == actualResult.Lifestyle.Id, errorMessagePrefix + " " + "Lifestyle is not returned properly.");
            Assert.True(expectedResult.HasAdditionalInfo == actualResult.HasAdditionalInfo, errorMessagePrefix + " " + "HasAdditionalInfo is not returned properly.");
            Assert.True(expectedResult.Allergies.First().Allergen.Id == actualResult.Allergies.First().Allergen.Id, errorMessagePrefix + " " + "Allergen is not returned properly.");
            Assert.True(expectedResult.Recipes.First().Id == actualResult.Recipes.First().Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            Assert.True(expectedResult.FavoriteRecipes.First().Recipe.Id == actualResult.FavoriteRecipes.First().Recipe.Id, errorMessagePrefix + " " + "FavoriteRecipe is not returned properly.");
            Assert.True(expectedResult.CookedRecipes.First().Recipe.Id == actualResult.CookedRecipes.First().Recipe.Id, errorMessagePrefix + " " + "CookedRecipe is not returned properly.");
            Assert.True(expectedResult.Reviews.First().Id == actualResult.Reviews.First().Id, errorMessagePrefix + " " + "Review is not returned properly.");
            Assert.True(expectedResult.ShoppingLists.First().ShoppingList.Id == actualResult.ShoppingLists.First().ShoppingList.Id, errorMessagePrefix + " " + "ShoppingList is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.GetByIdAsync(nonExistentUserId);
            });
        }

        [Fact]
        public async Task GetAdditionalInfoByUserIdAsync_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "UserService GetAdditionalInfoByUserIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            await this.SeedDataAsync(context);
            var user = userRepository.All().First();
            user.FullName = "FullName";
            user.Biography = "Biography";
            user.ProfilePhoto = "ProfilePhoto";
            user.Lifestyle = context.Lifestyles.First();
            user.Allergies.Add(new UserAllergen()
            {
                Allergen = context.Allergens.First(),
            });
            userRepository.Update(user);
            await userRepository.SaveChangesAsync();

            // Act
            var actualResult = await userService.GetAdditionalInfoByUserIdAsync(user.Id);
            var expectedResult = userRepository
                .All()
                .First()
                .To<UserAdditionalInfoServiceModel>();

            // Assert
            Assert.True(expectedResult.FullName == actualResult.FullName, errorMessagePrefix + " " + "FullName is not returned properly.");
            Assert.True(expectedResult.Biography == actualResult.Biography, errorMessagePrefix + " " + "Biography is not returned properly.");
            Assert.True(expectedResult.ProfilePhoto == actualResult.ProfilePhoto, errorMessagePrefix + " " + "ProfilePhoto is not returned properly.");
            Assert.True(expectedResult.Lifestyle.Id == actualResult.Lifestyle.Id, errorMessagePrefix + " " + "Lifestyle is not returned properly.");
            Assert.True(expectedResult.Allergies.First().Allergen.Id == actualResult.Allergies.First().Allergen.Id, errorMessagePrefix + " " + "Allergen is not returned properly.");
        }

        [Fact]
        public async Task GetAdditionalInfoByUserIdAsync_WithNonExistentUserId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(context);
            var userService = this.GetUserService(userRepository, context);
            var nonExistentUserId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await userService.GetAdditionalInfoByUserIdAsync(nonExistentUserId);
            });
        }

        private async Task SeedDataForGetByIdMethod(ApplicationDbContext context)
        {
            await context.Lifestyles.AddAsync(new Lifestyle());
            await context.Allergens.AddAsync(new Allergen());
            await context.Recipes.AddAsync(new Recipe());
            await context.Reviews.AddAsync(new Review());
            await context.ShoppingLists.AddAsync(new ShoppingList());
            await context.SaveChangesAsync();

            var user = new ApplicationUser()
            {
                FullName = "FullName",
                Biography = "Biography",
                ProfilePhoto = "ProfilePhoto",
                Lifestyle = context.Lifestyles.First(),
                HasAdditionalInfo = true,
            };
            user.Allergies.Add(new UserAllergen() { Allergen = context.Allergens.First() });
            user.Recipes.Add(context.Recipes.First());
            user.FavoriteRecipes.Add(new UserFavoriteRecipe() { Recipe = context.Recipes.First() });
            user.CookedRecipes.Add(new UserCookedRecipe() { Recipe = context.Recipes.First() });
            user.Reviews.Add(context.Reviews.First());
            user.ShoppingLists.Add(new UserShoppingList() { ShoppingList = context.ShoppingLists.First() });

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        private async Task SeedDataAsync(ApplicationDbContext context)
        {
            var lifestyle = new Lifestyle();
            await context.Lifestyles.AddAsync(lifestyle);

            var allergen = new Allergen();
            await context.Allergens.AddAsync(allergen);

            var user = new ApplicationUser();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        private UserService GetUserService(EfDeletableEntityRepository<ApplicationUser> userRepository, ApplicationDbContext context)
        {
            // UserManager
            var userManagerMock = this.GetUserManagerMock(context);

            // LifestyleService
            var lifestyleServiceMock = new Mock<ILifestyleService>();
            lifestyleServiceMock
                .Setup(x => x.SetLifestyleToUserAsync(It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Callback((string lifestyleType, ApplicationUser user) =>
                {
                    user.Lifestyle = context.Lifestyles.First(x => x.Type == lifestyleType);
                })
                .Returns(Task.FromResult(0));
            lifestyleServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int lifestyleId) => context.Lifestyles.First(x => x.Id == lifestyleId).To<LifestyleServiceModel>());

            // AllergenService
            var allergenServiceMock = new Mock<IAllergenService>();
            allergenServiceMock
                .Setup(x => x.SetAllergenToUserAsync(It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Callback((string allergenName, ApplicationUser user) =>
                {
                    user.Allergies.Add(new UserAllergen() { Allergen = context.Allergens.First(x => x.Name == allergenName) });
                })
                .Returns(Task.FromResult(0));

            // UserAllergenService
            var userAllergenServiceMock = new Mock<IUserAllergenService>();
            userAllergenServiceMock
                .Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
                .Returns((string userId) =>
                    Task.FromResult((ICollection<UserAllergenServiceModel>)context.UserAllergens
                    .Where(x => x.UserId == userId)
                    .To<UserAllergenServiceModel>()
                    .ToList()));

            // ShoppingListService
            var shoppingListServiceMock = new Mock<IShoppingListService>();
            shoppingListServiceMock
                .Setup(x => x.SetShoppingListToUserAsync(It.IsAny<string>(), It.IsAny<ApplicationUser>()))
                .Callback((string shoppingListId, ApplicationUser user) =>
                {
                    user.ShoppingLists.Add(new UserShoppingList
                    {
                        ShoppingList = context.ShoppingLists.First(x => x.Id == shoppingListId),
                    });
                })
                .Returns(Task.FromResult(0));

            var userService = new UserService(
                userManagerMock.Object,
                userRepository,
                lifestyleServiceMock.Object,
                allergenServiceMock.Object,
                userAllergenServiceMock.Object,
                shoppingListServiceMock.Object);

            return userService;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock(ApplicationDbContext context)
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            userManagerMock
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser user) =>
                {
                    context.Update(user);
                })
                .ReturnsAsync(IdentityResult.Success);

            return userManagerMock;
        }
    }
}
