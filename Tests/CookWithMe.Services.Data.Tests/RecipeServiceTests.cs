namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Common;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.NutritionalValues;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Tests.Common;
    using CookWithMe.Services.Data.Tests.Common.Seeders;
    using CookWithMe.Services.Data.Users;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Allergens;
    using CookWithMe.Services.Models.Categories;
    using CookWithMe.Services.Models.Lifestyles;
    using CookWithMe.Services.Models.NutritionalValues;
    using CookWithMe.Services.Models.Recipes;
    using CookWithMe.Services.Models.ShoppingLists;
    using CookWithMe.Services.Models.Users;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    using Xunit;

    public class RecipeServiceTests
    {
        [Fact]
        public async Task CreateAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForCreateAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = new RecipeServiceModel
            {
                CreatedOn = DateTime.UtcNow,
                Title = "Title",
                Photo = "Photo",
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
                Allergens = new HashSet<RecipeAllergenServiceModel>()
                {
                    new RecipeAllergenServiceModel
                    {
                        Allergen = new AllergenServiceModel() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
                {
                    new RecipeLifestyleServiceModel
                    {
                        Lifestyle = new LifestyleServiceModel() { Type = "LifestyleType" },
                    },
                },
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValueServiceModel(),
                UserId = context.Users.First().Id,
            };

            // Act
            var result = await recipeService.CreateAsync(recipeServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task CreateAsync_WithCorrectData_ShouldSuccessfullyCreate()
        {
            var errorMessagePrefix = "RecipeService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForCreateAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = new RecipeServiceModel
            {
                Title = "Title",
                Photo = "Photo",
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
                Allergens = new HashSet<RecipeAllergenServiceModel>()
                {
                    new RecipeAllergenServiceModel
                    {
                        Allergen = new AllergenServiceModel() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
                {
                    new RecipeLifestyleServiceModel
                    {
                        Lifestyle = new LifestyleServiceModel() { Type = "LifestyleType" },
                    },
                },
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValueServiceModel(),
                UserId = context.Users.First().Id,
            };

            // Act
            await recipeService.CreateAsync(recipeServiceModel);
            var actualResult = recipeRepository.All().First();
            var expectedResult = recipeServiceModel;

            // Assert
            Assert.True(expectedResult.Title == actualResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedResult.Photo == actualResult.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            Assert.True(expectedResult.Category.Title == actualResult.Category.Title, errorMessagePrefix + " " + "Category Title is not returned properly.");
            Assert.True(expectedResult.Summary == actualResult.Summary, errorMessagePrefix + " " + "Summary is not returned properly.");
            Assert.True(expectedResult.Directions == actualResult.Directions, errorMessagePrefix + " " + "Directions are not returned properly.");
            Assert.True(expectedResult.Allergens.First().Allergen.Name == actualResult.Allergens.First().Allergen.Name, errorMessagePrefix + " " + "AllergenName is not returned properly.");
            Assert.True(expectedResult.Lifestyles.First().Lifestyle.Type == actualResult.Lifestyles.First().Lifestyle.Type, errorMessagePrefix + " " + "LifestyleType is not returned properly.");
            Assert.True(expectedResult.SkillLevel == actualResult.SkillLevel, errorMessagePrefix + " " + "SkillLevel is not returned properly.");
            Assert.True(expectedResult.PreparationTime == actualResult.PreparationTime, errorMessagePrefix + " " + "PreparationTime is not returned properly.");
            Assert.True(expectedResult.CookingTime == actualResult.CookingTime, errorMessagePrefix + " " + "CookingTime is not returned properly.");
            Assert.True(expectedResult.NeededTime == actualResult.NeededTime, errorMessagePrefix + " " + "NeededTime is not returned properly.");
            Assert.True(expectedResult.Serving == actualResult.Serving, errorMessagePrefix + " " + "Serving is not returned properly.");
            Assert.True(expectedResult.UserId == actualResult.UserId, errorMessagePrefix + " " + "UserId is not returned properly.");

            Assert.True(actualResult.ShoppingListId == context.ShoppingLists.First().Id, errorMessagePrefix + " " + "ShoppingListId is not set properly.");
            Assert.True(actualResult.NutritionalValueId == context.NutritionalValues.First().Id, errorMessagePrefix + " " + "NutritionalValueId is not set properly.");
        }

        [Fact]
        public async Task CreateAsync_WithIncorrectProperty_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForCreateAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = new RecipeServiceModel
            {
                Title = null,
                Photo = "Photo",
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
                Allergens = new HashSet<RecipeAllergenServiceModel>()
                {
                    new RecipeAllergenServiceModel
                    {
                        Allergen = new AllergenServiceModel() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
                {
                    new RecipeLifestyleServiceModel
                    {
                        Lifestyle = new LifestyleServiceModel() { Type = "LifestyleType" },
                    },
                },
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValueServiceModel(),
                UserId = context.Users.First().Id,
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.CreateAsync(recipeServiceModel);
            });
        }

        [Fact]
        public async Task CreateAsync_WithEmptyLifestylesCollection_ShouldThrowEmptyCollectionException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForCreateAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = new RecipeServiceModel
            {
                Title = "Title",
                Photo = "Photo",
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
                Allergens = new HashSet<RecipeAllergenServiceModel>()
                {
                    new RecipeAllergenServiceModel
                    {
                        Allergen = new AllergenServiceModel() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyleServiceModel>(),
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValueServiceModel(),
                UserId = context.Users.First().Id,
            };

            // Act

            // Assert
            await Assert.ThrowsAsync<EmptyCollectionException>(async () =>
            {
                await recipeService.CreateAsync(recipeServiceModel);
            });
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForEditAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = recipeRepository.All().First().To<RecipeServiceModel>();
            recipeServiceModel.Title = "Edited Title";
            recipeServiceModel.Category = context.Categories.First(x => x.Title == "Edited CategoryTitle").To<CategoryServiceModel>();
            recipeServiceModel.Summary = "Edited Summary";
            recipeServiceModel.Directions = "Edited Directions";
            recipeServiceModel.ShoppingList.Ingredients = "Edited Ingredients";
            recipeServiceModel.Allergens = new HashSet<RecipeAllergenServiceModel>()
            {
                new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel() { Name = "Edited AllergenName" },
                },
            };
            recipeServiceModel.Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
            {
                new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel() { Type = "Edited LifestyleType" },
                },
            };
            recipeServiceModel.SkillLevel = Level.Medium;
            recipeServiceModel.PreparationTime = 20;
            recipeServiceModel.CookingTime = 20;
            recipeServiceModel.NeededTime = Period.HalfAnHour;
            recipeServiceModel.Serving = Size.Two;
            recipeServiceModel.NutritionalValue.Calories = 10;
            recipeServiceModel.Yield = 10;

            // Act
            var result = await recipeService.EditAsync(recipeServiceModel.Id, recipeServiceModel);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task EditAsync_WithCorrectData_ShouldSuccessfullyEdit()
        {
            var errorMessagePrefix = "RecipeService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForEditAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = recipeRepository.All().First().To<RecipeServiceModel>();
            recipeServiceModel.Title = "Edited Title";
            recipeServiceModel.Category = context.Categories.First(x => x.Title == "Edited CategoryTitle").To<CategoryServiceModel>();
            recipeServiceModel.Summary = "Edited Summary";
            recipeServiceModel.Directions = "Edited Directions";
            recipeServiceModel.ShoppingList.Ingredients = "Edited Ingredients";
            recipeServiceModel.Allergens = new HashSet<RecipeAllergenServiceModel>()
            {
                new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel() { Name = "Edited AllergenName" },
                },
            };
            recipeServiceModel.Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
            {
                new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel() { Type = "Edited LifestyleType" },
                },
            };
            recipeServiceModel.SkillLevel = Level.Medium;
            recipeServiceModel.PreparationTime = 20;
            recipeServiceModel.CookingTime = 20;
            recipeServiceModel.NeededTime = Period.HalfAnHour;
            recipeServiceModel.Serving = Size.Two;
            recipeServiceModel.NutritionalValue.Calories = 10;
            recipeServiceModel.Yield = 10;

            // Act
            await recipeService.EditAsync(recipeServiceModel.Id, recipeServiceModel);
            var actualResult = recipeRepository.All().First();
            var expectedResult = recipeServiceModel;

            // Assert
            Assert.True(expectedResult.Title == actualResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedResult.Category.Title == actualResult.Category.Title, errorMessagePrefix + " " + "Category Title is not returned properly.");
            Assert.True(expectedResult.Summary == actualResult.Summary, errorMessagePrefix + " " + "Summary is not returned properly.");
            Assert.True(expectedResult.Directions == actualResult.Directions, errorMessagePrefix + " " + "Directions are not returned properly.");
            Assert.True(expectedResult.ShoppingList.Ingredients == actualResult.ShoppingList.Ingredients, errorMessagePrefix + " " + "ShoppingList Ingredients are not returned properly.");
            Assert.True(expectedResult.Allergens.First().Allergen.Name == actualResult.Allergens.First().Allergen.Name, errorMessagePrefix + " " + "AllergenName is not returned properly.");
            Assert.True(expectedResult.Lifestyles.First().Lifestyle.Type == actualResult.Lifestyles.First().Lifestyle.Type, errorMessagePrefix + " " + "LifestyleType is not returned properly.");
            Assert.True(expectedResult.SkillLevel == actualResult.SkillLevel, errorMessagePrefix + " " + "SkillLevel is not returned properly.");
            Assert.True(expectedResult.PreparationTime == actualResult.PreparationTime, errorMessagePrefix + " " + "PreparationTime is not returned properly.");
            Assert.True(expectedResult.CookingTime == actualResult.CookingTime, errorMessagePrefix + " " + "CookingTime is not returned properly.");
            Assert.True(expectedResult.NeededTime == actualResult.NeededTime, errorMessagePrefix + " " + "NeededTime is not returned properly.");
            Assert.True(expectedResult.Serving == actualResult.Serving, errorMessagePrefix + " " + "Serving is not returned properly.");
            Assert.True(expectedResult.NutritionalValue.Calories == actualResult.NutritionalValue.Calories, errorMessagePrefix + " " + ".NutritionalValue Calories are not returned properly.");
            Assert.True(expectedResult.Yield == actualResult.Yield, errorMessagePrefix + " " + "Yield is not returned properly.");
        }

        [Fact]
        public async Task EditAsync_WithNonExisterntId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = new RecipeServiceModel();
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.EditAsync(nonExistentId, recipeServiceModel);
            });
        }

        [Fact]
        public async Task EditAsync_WithIncorrectProperty_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForEditAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = recipeRepository.All().First().To<RecipeServiceModel>();
            recipeServiceModel.Title = null;
            recipeServiceModel.Category = context.Categories.First(x => x.Title == "Edited CategoryTitle").To<CategoryServiceModel>();
            recipeServiceModel.Summary = "Edited Summary";
            recipeServiceModel.Directions = "Edited Directions";
            recipeServiceModel.ShoppingList.Ingredients = "Edited Ingredients";
            recipeServiceModel.Allergens = new HashSet<RecipeAllergenServiceModel>()
            {
                new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel() { Name = "Edited AllergenName" },
                },
            };
            recipeServiceModel.Lifestyles = new HashSet<RecipeLifestyleServiceModel>()
            {
                new RecipeLifestyleServiceModel
                {
                    Lifestyle = new LifestyleServiceModel() { Type = "Edited LifestyleType" },
                },
            };
            recipeServiceModel.SkillLevel = Level.Medium;
            recipeServiceModel.PreparationTime = 20;
            recipeServiceModel.CookingTime = 20;
            recipeServiceModel.NeededTime = Period.HalfAnHour;
            recipeServiceModel.Serving = Size.Two;
            recipeServiceModel.NutritionalValue.Calories = 10;
            recipeServiceModel.Yield = 10;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.EditAsync(recipeServiceModel.Id, recipeServiceModel);
            });
        }

        [Fact]
        public async Task EditAsync_WithEmptyLifestylesCollection_ShouldThrowEmptyCollectionException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForEditAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeServiceModel = recipeRepository.All().First().To<RecipeServiceModel>();
            recipeServiceModel.Title = "Edited Title";
            recipeServiceModel.Category = context.Categories.First(x => x.Title == "Edited CategoryTitle").To<CategoryServiceModel>();
            recipeServiceModel.Summary = "Edited Summary";
            recipeServiceModel.Directions = "Edited Directions";
            recipeServiceModel.ShoppingList.Ingredients = "Edited Ingredients";
            recipeServiceModel.Allergens = new HashSet<RecipeAllergenServiceModel>()
            {
                new RecipeAllergenServiceModel
                {
                    Allergen = new AllergenServiceModel() { Name = "Edited AllergenName" },
                },
            };
            recipeServiceModel.Lifestyles = new HashSet<RecipeLifestyleServiceModel>();
            recipeServiceModel.SkillLevel = Level.Medium;
            recipeServiceModel.PreparationTime = 20;
            recipeServiceModel.CookingTime = 20;
            recipeServiceModel.NeededTime = Period.HalfAnHour;
            recipeServiceModel.Serving = Size.Two;
            recipeServiceModel.NutritionalValue.Calories = 10;
            recipeServiceModel.Yield = 10;

            // Act

            // Assert
            await Assert.ThrowsAsync<EmptyCollectionException>(async () =>
            {
                await recipeService.EditAsync(recipeServiceModel.Id, recipeServiceModel);
            });
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var existentId = recipeRepository.All().First().Id;

            // Act
            var result = await recipeService.DeleteByIdAsync(existentId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithExistentId_ShouldSuccessfullyDelete()
        {
            var errorMessagePrefix = "RecipeService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var existentId = recipeRepository.All().First().Id;

            // Act
            var recipesCount = recipeRepository.All().Count();
            await recipeService.DeleteByIdAsync(existentId);
            var actualResult = recipeRepository.All().Count();
            var expectedResult = recipesCount - 1;

            // Assert
            Assert.True(actualResult == expectedResult, errorMessagePrefix + " " + "Recipes count is not reduced.");
        }

        [Fact]
        public async Task DeleteByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.DeleteByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task GetByIdAsync_WithExistentId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var existentId = recipeRepository.All().First().Id;

            // Act
            var actualResult = await recipeService.GetByIdAsync(existentId);
            var expectedResult = (await recipeRepository
                .All()
                .SingleOrDefaultAsync(x => x.Id == existentId))
                .To<RecipeServiceModel>();

            // Assert
            Assert.True(actualResult.Id == expectedResult.Id, errorMessagePrefix + " " + "Id is not returned properly.");
            Assert.True(expectedResult.Title == actualResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedResult.CategoryId == actualResult.CategoryId, errorMessagePrefix + " " + "CategoryId is not returned properly.");
            Assert.True(expectedResult.Summary == actualResult.Summary, errorMessagePrefix + " " + "Summary is not returned properly.");
            Assert.True(expectedResult.Directions == actualResult.Directions, errorMessagePrefix + " " + "Directions are not returned properly.");
            Assert.True(expectedResult.ShoppingListId == actualResult.ShoppingListId, errorMessagePrefix + " " + "ShoppingListId is not returned properly.");
            Assert.True(expectedResult.SkillLevel == actualResult.SkillLevel, errorMessagePrefix + " " + "SkillLevel is not returned properly.");
            Assert.True(expectedResult.PreparationTime == actualResult.PreparationTime, errorMessagePrefix + " " + "PreparationTime is not returned properly.");
            Assert.True(expectedResult.CookingTime == actualResult.CookingTime, errorMessagePrefix + " " + "CookingTime is not returned properly.");
            Assert.True(expectedResult.NeededTime == actualResult.NeededTime, errorMessagePrefix + " " + "NeededTime is not returned properly.");
            Assert.True(expectedResult.Serving == actualResult.Serving, errorMessagePrefix + " " + "Serving is not returned properly.");
            Assert.True(expectedResult.NutritionalValueId == actualResult.NutritionalValueId, errorMessagePrefix + " " + "NutritionalValueId is not returned properly.");
            Assert.True(expectedResult.Yield == actualResult.Yield, errorMessagePrefix + " " + "Yield is not returned properly.");
            Assert.True(expectedResult.UserId == actualResult.UserId, errorMessagePrefix + " " + "UserId is not returned properly.");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistentId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var nonExistentId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.GetByIdAsync(nonExistentId);
            });
        }

        [Fact]
        public async Task SetRecipeToUserFavoriteRecipesAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "RecipeService SetRecipeToUserFavoriteRecipesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeId = recipeRepository.All().First().Id;
            var userId = context.Users.First().Id;

            // Act
            var result = await recipeService.SetRecipeToUserFavoriteRecipesAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task SetRecipeToUserFavoriteRecipesAsync_WithNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();
            var userId = context.Users.First().Id;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.SetRecipeToUserFavoriteRecipesAsync(userId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task SetRecipeToUserCookedRecipesAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "RecipeService SetRecipeToUserCookedRecipesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeId = recipeRepository.All().First().Id;
            var userId = context.Users.First().Id;

            // Act
            var result = await recipeService.SetRecipeToUserCookedRecipesAsync(userId, recipeId);

            // Assert
            Assert.True(result, errorMessagePrefix + " " + "Returns false.");
        }

        [Fact]
        public async Task SetRecipeToUserCookedRecipesAsync_WithNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var nonExistentRecipeId = Guid.NewGuid().ToString();
            var userId = context.Users.First().Id;

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.SetRecipeToUserCookedRecipesAsync(userId, nonExistentRecipeId);
            });
        }

        [Fact]
        public async Task SetRecipeToReviewAsync_WithCorrectData_ShouldSuccessfullySet()
        {
            var errorMessagePrefix = "RecipeService SetRecipeToReviewAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipeAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var review = new Review();
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();
            var recipeId = recipeRepository.All().First().Id;

            // Act
            await recipeService.SetRecipeToReviewAsync(recipeId, context.Reviews.First());
            var actualResult = context.Reviews.First().Recipe.Id;
            var expectedResult = recipeId;

            // Assert
            Assert.True(expectedResult == actualResult, errorMessagePrefix + " " + "Recipe is not set properly.");
        }

        [Fact]
        public async Task SetRecipeToReviewAsync_WithNonExistentRecipeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var review = new Review();
            await context.Reviews.AddAsync(review);
            await context.SaveChangesAsync();
            var nonExistentRecipeId = Guid.NewGuid().ToString();

            // Act

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await recipeService.SetRecipeToReviewAsync(nonExistentRecipeId, context.Reviews.First());
            });
        }

        [Fact]
        public async Task GetByCategoryId_WithExistentCategoryId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetByCategoryId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var categoryId = context.Categories.First(x => x.Title == "Category 1").Id;

            // Act
            var actualResult = await recipeService.GetByCategoryId(categoryId).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByCategoryId_WithExistentCategoryIdWithNoRecipes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetByCategoryId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var categoryId = context.Categories.First(x => x.Title == "Empty category").Id;

            // Act
            var result = await recipeService.GetByCategoryId(categoryId).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(result.Count == 0, errorMessagePrefix + " " + "Collection is not empty.");
        }

        [Fact]
        public async Task GetByUserId_WithExistentUserId_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var userId = context.Users.First(x => x.Biography == "User with 2 recipes").Id;

            // Act
            var actualResult = await recipeService.GetByUserId(userId).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByUserId_WithExistentUserIdWithNoRecipes_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedRecipesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var userId = context.Users.First(x => x.Biography == "User with 0 recipes").Id;

            // Act
            var result = await recipeService.GetByUserId(userId).ToListAsync();

            // Assert
            Assert.True(result.Count == 0, errorMessagePrefix + " " + "Collection is not empty.");
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedLifestyleAndAllergies_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndAllergiesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeServiceForGetAllFilteredAsyncMethod(recipeRepository, context);
            var userId = context.Users.First().Id;

            // Act
            var actualResult = await recipeService.GetAllFilteredAsync(userId);
            var expectedResult = recipeRepository
                .All()
                .First(x => x.Title == "Vegetarian Recipe without Milk");

            // Assert
            Assert.True(actualResult.Count() == 1, errorMessagePrefix + " " + "Collections count mismatch.");
            Assert.True(expectedResult.Id == actualResult.First().Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedLifestyleAndNoAllergies_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndNoAllergiesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeServiceForGetAllFilteredAsyncMethod(recipeRepository, context);
            var userId = context.Users.First().Id;

            // Act
            var actualResult = await (await recipeService.GetAllFilteredAsync(userId)).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegetarian Recipe without Milk" ||
                            x.Title == "Vegetarian Recipe with Milk")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedAllergiesAndNoLifestyle_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedAllergiesAndNoLifestyleAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeServiceForGetAllFilteredAsyncMethod(recipeRepository, context);
            var userId = context.Users.First().Id;

            // Act
            var actualResult = await (await recipeService.GetAllFilteredAsync(userId)).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegetarian Recipe without Milk" ||
                            x.Title == "Not Vegetarian Recipe without Milk")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithNoSpecifiedLifestyleAndNoAllergies_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetAllFilteredAsyncMethodWithNoSpecifiedLifestyleAndNoAllergiesAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeServiceForGetAllFilteredAsyncMethod(recipeRepository, context);
            var userId = context.Users.First().Id;

            // Act
            var actualResult = await (await recipeService.GetAllFilteredAsync(userId)).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedKeyWords_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                KeyWords = "vegetarian breakfast",
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegan breakfast with peanuts" ||
                            x.Title == "Vegetarian breakfast with milk" ||
                            x.Title == "Vegetarian dessert with milk")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedKeyWordsAndCategory_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = context.Categories
                    .First(x => x.Title == "Breakfast")
                    .To<CategoryServiceModel>(),
                Lifestyle = new LifestyleServiceModel(),
                KeyWords = "vegetarian breakfast",
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegan breakfast with peanuts" ||
                            x.Title == "Vegetarian breakfast with milk")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedKeyWordsCategoryAndLifestyle_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = context.Categories
                    .First(x => x.Title == "Breakfast")
                    .To<CategoryServiceModel>(),
                Lifestyle = context.Lifestyles
                    .First(x => x.Type == "Vegetarian")
                    .To<LifestyleServiceModel>(),
                KeyWords = "vegetarian breakfast",
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegetarian breakfast with milk")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedCategoryLifestyleAndNoKeyWords_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = context.Categories
                    .First(x => x.Title == "Breakfast")
                    .To<CategoryServiceModel>(),
                Lifestyle = context.Lifestyles
                    .First(x => x.Type == "Vegetarian")
                    .To<LifestyleServiceModel>(),
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Category.Title == "Breakfast" && x.Lifestyles.Select(y => y.Lifestyle.Type).Contains("Vegetarian"))
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedAllergen_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                Allergens = new List<RecipeAllergenServiceModel>(),
            };
            recipeBrowseServiceModel.Allergens.Add(new RecipeAllergenServiceModel()
            {
                Allergen = context.Allergens.First(x => x.Name == "Milk")
                    .To<AllergenServiceModel>(),
            });

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => !x.Allergens.Select(y => y.Allergen.Name).Contains("Milk"))
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedCategoryLifestyleAndAllergen_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = context.Categories.First(x => x.Title == "Breakfast")
                    .To<CategoryServiceModel>(),
                Lifestyle = context.Lifestyles.First(x => x.Type == "Vegan")
                    .To<LifestyleServiceModel>(),
                Allergens = new List<RecipeAllergenServiceModel>(),
            };
            recipeBrowseServiceModel.Allergens.Add(new RecipeAllergenServiceModel()
            {
                Allergen = context.Allergens.First(x => x.Name == "Milk")
                    .To<AllergenServiceModel>(),
            });

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegan breakfast with peanuts")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedSkillLevel_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                SkillLevel = Level.Easy,
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.SkillLevel == Level.Easy)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedServing_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                Serving = Size.Two,
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Serving == Size.Two)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedNeededTime_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                NeededTime = Period.HalfAnHour,
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.NeededTime == Period.HalfAnHour)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedNutritionalValueCalories_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                NutritionalValue = new NutritionalValueServiceModel()
                {
                    Calories = 300,
                },
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.NutritionalValue.Calories == 300)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedNutritionalValueCaloriesAndFats_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                NutritionalValue = new NutritionalValueServiceModel()
                {
                    Calories = 300,
                    Fats = 10,
                },
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.NutritionalValue.Calories == 300 &&
                            x.NutritionalValue.Fats == 10)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedYield_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                Yield = 12,
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Yield == 12)
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithSpecifiedKeyWordsNutritionalValueAndYield_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                KeyWords = "vegan food",
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
                NutritionalValue = new NutritionalValueServiceModel()
                {
                    Calories = 300,
                },
                Yield = 12,
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => x.Title == "Vegan breakfast with peanuts" ||
                            x.Title == "Vegan dessert with peanuts")
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetBySearchValuesAsync_WithNoSpecifications_ShouldReturnCorrectResult()
        {
            var errorMessagePrefix = "RecipeService GetBySearchValuesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var seeder = new RecipeServiceTestsSeeder();
            await seeder.SeedDataForGetBySearchValuesAsyncMethodAsync(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var recipeBrowseServiceModel = new RecipeBrowseServiceModel()
            {
                Category = new CategoryServiceModel(),
                Lifestyle = new LifestyleServiceModel(),
            };

            // Act
            var actualResult = await (await recipeService.GetBySearchValuesAsync(recipeBrowseServiceModel))
                .ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            Assert.True(expectedResult.Count == actualResult.Count, errorMessagePrefix + " " + "Collections count mismatch.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        private RecipeService GetRecipeServiceForGetAllFilteredAsyncMethod(EfDeletableEntityRepository<Recipe> recipeRepository, ApplicationDbContext context)
        {
            // CategoryService
            var categoryServiceMock = new Mock<ICategoryService>();

            // LifestyleService
            var lifestyleServiceMock = new Mock<ILifestyleService>();

            // RecipeLifestyleService
            var recipeLifestyleServiceMock = new Mock<IRecipeLifestyleService>();

            // UserService
            var userServiceMock = new Mock<IUserService>();
            userServiceMock
               .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
               .Returns(Task.FromResult<ApplicationUserServiceModel>(context.Users.First().To<ApplicationUserServiceModel>()));

            // ShoppingListService
            var shoppingListServiceMock = new Mock<IShoppingListService>();

            // UserShoppingListService
            var userShoppingListServiceMock = new Mock<IUserShoppingListService>();

            // NutritionalValueService
            var nutritionalValueServiceMock = new Mock<INutritionalValueService>();

            // AllergenService
            var allergenServiceMock = new Mock<IAllergenService>();

            // RecipeAllergenService
            var recipeAllergenServiceMock = new Mock<IRecipeAllergenService>();
            recipeAllergenServiceMock
              .Setup(x => x.GetRecipeIdsByAllergenIdsAsync(It.IsAny<IEnumerable<int>>()))
              .Returns((IEnumerable<int> allergenIds) =>
                    Task.FromResult((ICollection<string>)context.RecipeAllergens
                        .Where(x => allergenIds.Contains(x.AllergenId))
                        .Select(x => x.RecipeId)
                        .ToList()));

            // UserFavoriteRecipeService
            var userFavoriteRecipeServiceMock = new Mock<IUserFavoriteRecipeService>();

            // UserCookedRecipeService
            var userCookedRecipeServiceMock = new Mock<IUserCookedRecipeService>();

            // UserAllergenService
            var userAllergenServiceMock = new Mock<IUserAllergenService>();
            userAllergenServiceMock
               .Setup(x => x.GetByUserIdAsync(It.IsAny<string>()))
               .Returns((string userId) =>
                    Task.FromResult((ICollection<UserAllergenServiceModel>)context.UserAllergens
                        .Where(x => x.UserId == userId)
                        .To<UserAllergenServiceModel>()
                        .ToList()));

            // StringFormatService
            var stringFormatServiceMock = new Mock<IStringFormatService>();

            // EnumParseService
            var enumParseServiceMock = new Mock<IEnumParseService>();

            var recipeService = new RecipeService(
                recipeRepository,
                categoryServiceMock.Object,
                lifestyleServiceMock.Object,
                recipeLifestyleServiceMock.Object,
                userServiceMock.Object,
                shoppingListServiceMock.Object,
                userShoppingListServiceMock.Object,
                nutritionalValueServiceMock.Object,
                allergenServiceMock.Object,
                recipeAllergenServiceMock.Object,
                userFavoriteRecipeServiceMock.Object,
                userCookedRecipeServiceMock.Object,
                userAllergenServiceMock.Object,
                stringFormatServiceMock.Object,
                enumParseServiceMock.Object);

            return recipeService;
        }

        private RecipeService GetRecipeService(EfDeletableEntityRepository<Recipe> recipeRepository, ApplicationDbContext context)
        {
            // CategoryService
            var categoryServiceMock = new Mock<ICategoryService>();
            categoryServiceMock
              .Setup(x => x.SetCategoryToRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
              .Callback((string categoryTitle, Recipe recipe) =>
              {
                  recipe.Category = context.Categories.First(x => x.Title == categoryTitle);
              })
              .Returns(Task.FromResult(0));
            categoryServiceMock
              .Setup(x => x.GetIdByTitleAsync(It.IsAny<string>()))
              .ReturnsAsync((string categoryTitle) => context.Categories.First(x => x.Title == categoryTitle).Id);

            // LifestyleService
            var lifestyleServiceMock = new Mock<ILifestyleService>();
            lifestyleServiceMock
              .Setup(x => x.SetLifestyleToRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
              .Callback((string lifestyleType, Recipe recipe) =>
              {
                  recipe.Lifestyles.Add(new RecipeLifestyle() { Lifestyle = context.Lifestyles.First(x => x.Type == lifestyleType) });
              })
              .Returns(Task.FromResult(0));
            lifestyleServiceMock
              .Setup(x => x.GetIdByTypeAsync(It.IsAny<string>()))
              .ReturnsAsync((string lifestyleType) => context.Lifestyles.First(x => x.Type == lifestyleType).Id);

            // RecipeLifestyleService
            var recipeLifestyleServiceMock = new Mock<IRecipeLifestyleService>();
            recipeLifestyleServiceMock
                .Setup(x => x.DeletePreviousRecipeLifestylesByRecipeId(It.IsAny<string>()))
                .Callback((string recipeId) =>
                {
                    context.RecipeLifestyles.RemoveRange(context.RecipeLifestyles.Where(x => x.RecipeId == recipeId));
                })
                .Verifiable();
            recipeLifestyleServiceMock
                .Setup(x => x.GetRecipeIdsByLifestyleIdAsync(It.IsAny<int>()))
                .Returns((int lifestyleId) =>
                    Task.FromResult((ICollection<string>)context.RecipeLifestyles
                        .Where(x => x.LifestyleId == lifestyleId)
                        .Select(x => x.RecipeId)
                        .ToList()));

            // UserService
            var userServiceMock = new Mock<IUserService>();
            userServiceMock
                .Setup(x => x.SetFavoriteRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
                .Returns(Task.FromResult(true));
            userServiceMock
               .Setup(x => x.SetCookedRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
               .Returns(Task.FromResult(true));

            // ShoppingListService
            var shoppingListServiceMock = new Mock<IShoppingListService>();
            shoppingListServiceMock
                .Setup(x => x.GetIdByRecipeIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string recipeId) => context.ShoppingLists.First(x => x.RecipeId == recipeId).Id);
            shoppingListServiceMock
               .Setup(x => x.EditAsync(It.IsAny<string>(), It.IsAny<ShoppingListServiceModel>()))
               .Callback((string id, ShoppingListServiceModel shoppingListServiceModel) =>
               {
                   context.ShoppingLists.First().Ingredients = shoppingListServiceModel.Ingredients;
                   context.Update(context.ShoppingLists.First());
               })
               .Returns(Task.FromResult(0));

            // UserShoppingListService
            var userShoppingListServiceMock = new Mock<IUserShoppingListService>();

            // NutritionalValueService
            var nutritionalValueServiceMock = new Mock<INutritionalValueService>();
            nutritionalValueServiceMock
                .Setup(x => x.GetIdByRecipeIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string recipeId) => context.NutritionalValues.First(x => x.RecipeId == recipeId).Id);
            nutritionalValueServiceMock
               .Setup(x => x.EditAsync(It.IsAny<string>(), It.IsAny<NutritionalValueServiceModel>()))
               .Callback((string id, NutritionalValueServiceModel nutritionalValueServiceModel) =>
               {
                   context.NutritionalValues.First().Calories = nutritionalValueServiceModel.Calories;
                   context.Update(context.NutritionalValues.First());
               })
               .Returns(Task.FromResult(0));

            // AllergenService
            var allergenServiceMock = new Mock<IAllergenService>();
            allergenServiceMock
               .Setup(x => x.SetAllergenToRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
               .Callback((string allergenName, Recipe recipe) =>
               {
                   recipe.Allergens.Add(new RecipeAllergen() { Allergen = context.Allergens.First(x => x.Name == allergenName) });
               })
               .Returns(Task.FromResult(0));
            allergenServiceMock
                .Setup(x => x.GetIdsByNamesAsync(It.IsAny<IEnumerable<string>>()))
                .Returns((IEnumerable<string> allergenNames) =>
                    Task.FromResult((IEnumerable<int>)context.Allergens
                        .Where(x => allergenNames.Contains(x.Name))
                        .Select(x => x.Id)
                        .ToList()));

            // RecipeAllergenService
            var recipeAllergenServiceMock = new Mock<IRecipeAllergenService>();
            recipeAllergenServiceMock
                .Setup(x => x.DeletePreviousRecipeAllergensByRecipeId(It.IsAny<string>()))
                .Callback((string recipeId) =>
                {
                    context.RecipeAllergens.RemoveRange(context.RecipeAllergens.Where(x => x.RecipeId == recipeId));
                })
                .Verifiable();
            recipeAllergenServiceMock
                .Setup(x => x.GetRecipeIdsByAllergenIdsAsync(It.IsAny<IEnumerable<int>>()))
                .Returns((IEnumerable<int> allergenIds) =>
                    Task.FromResult((ICollection<string>)context.RecipeAllergens
                        .Where(x => allergenIds.Contains(x.AllergenId))
                        .Select(x => x.RecipeId)
                        .ToList()));

            // UserFavoriteRecipeService
            var userFavoriteRecipeServiceMock = new Mock<IUserFavoriteRecipeService>();

            // UserCookedRecipeService
            var userCookedRecipeServiceMock = new Mock<IUserCookedRecipeService>();

            // UserAllergenService
            var userAllergenServiceMock = new Mock<IUserAllergenService>();

            // StringFormatService
            var stringFormatServiceMock = new Mock<IStringFormatService>();
            stringFormatServiceMock
                .Setup(x => x.SplitByCommaAndWhitespace(It.IsAny<string>()))
                .Returns((string text) => text.ToLower().Split(
                    new string[] { ",", " ", ", " },
                    StringSplitOptions.RemoveEmptyEntries));

            // EnumParseService
            var enumParseServiceMock = new Mock<IEnumParseService>();

            var recipeService = new RecipeService(
                recipeRepository,
                categoryServiceMock.Object,
                lifestyleServiceMock.Object,
                recipeLifestyleServiceMock.Object,
                userServiceMock.Object,
                shoppingListServiceMock.Object,
                userShoppingListServiceMock.Object,
                nutritionalValueServiceMock.Object,
                allergenServiceMock.Object,
                recipeAllergenServiceMock.Object,
                userFavoriteRecipeServiceMock.Object,
                userCookedRecipeServiceMock.Object,
                userAllergenServiceMock.Object,
                stringFormatServiceMock.Object,
                enumParseServiceMock.Object);

            return recipeService;
        }
    }
}
