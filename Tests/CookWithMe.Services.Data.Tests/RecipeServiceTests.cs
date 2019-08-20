namespace CookWithMe.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Common.Exceptions;
    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Models.Enums;
    using CookWithMe.Data.Repositories;
    using CookWithMe.Services.Data.Allergens;
    using CookWithMe.Services.Data.Categories;
    using CookWithMe.Services.Data.Lifestyles;
    using CookWithMe.Services.Data.NutritionalValues;
    using CookWithMe.Services.Data.Recipes;
    using CookWithMe.Services.Data.ShoppingLists;
    using CookWithMe.Services.Data.Tests.Common;
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
            string errorMessagePrefix = "RecipeService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForCreateAsyncMethod(context);
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
            string errorMessagePrefix = "RecipeService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForCreateAsyncMethod(context);
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
            await this.SeedDataForCreateAsyncMethod(context);
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
            await this.SeedDataForCreateAsyncMethod(context);
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
            string errorMessagePrefix = "RecipeService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForEditAsyncMethod(context);
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
            string errorMessagePrefix = "RecipeService EditAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForEditAsyncMethod(context);
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
            await this.SeedDataForEditAsyncMethod(context);
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
            await this.SeedDataForEditAsyncMethod(context);
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
            string errorMessagePrefix = "RecipeService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService DeleteByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService GetByIdAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService SetRecipeToUserFavoriteRecipesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService SetRecipeToUserCookedRecipesAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService SetRecipeToReviewAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipe(context);
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
            string errorMessagePrefix = "RecipeService GetByCategoryId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
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
            Assert.True(actualResult.Count == 2, errorMessagePrefix + " " + "Recipes count is not equal to 2.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByCategoryId_WithExistentCategoryIdWithNoRecipes_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetByCategoryId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
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
            Assert.True(result.Count == 0, errorMessagePrefix + " " + "Recipes count is not equal to 0.");
        }

        [Fact]
        public async Task GetByUserId_WithExistentUserId_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
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
            Assert.True(actualResult.Count == 2, errorMessagePrefix + " " + "Recipes count is not equal to 2.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByUserId_WithExistentUserIdWithNoRecipes_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetByUserId() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var userId = context.Users.First(x => x.Biography == "User with 0 recipes").Id;

            // Act
            var result = await recipeService.GetByUserId(userId).ToListAsync();

            // Assert
            Assert.True(result.Count == 0, errorMessagePrefix + " " + "Recipes count is not equal to 0.");
        }

        [Fact]
        public async Task GetByIds_WithExistentIds_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetByIds() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var ids = await recipeRepository.All().Select(x => x.Id).ToListAsync();

            // Act
            var actualResult = await recipeService.GetByIds(ids).ToListAsync();
            var expectedResult = await recipeRepository
                .All()
                .Where(x => ids.Contains(x.Id))
                .To<RecipeServiceModel>()
                .ToListAsync();

            // Assert
            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetByIds_WithNonExistentIds_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetByIds() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedRecipes(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository, context);
            var nonExistentIds = new List<string>()
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
            };

            // Act
            var result = await recipeService.GetByIds(nonExistentIds).ToListAsync(); ;

            // Assert
            Assert.True(result.Count == 0, errorMessagePrefix + " " + "Recipes count is not equal to 0.");
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedLifestyleAndAllergies_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndAllergies(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeServiceForGetAllFilteredAsyncMethod(recipeRepository, context);
            var userId = context.Users.First().Id;

            // Act
            var actualResult = await recipeService.GetAllFilteredAsync(userId);
            var expectedResult = recipeRepository
                .All()
                .First(x => x.Title == "Vegetarian Recipe without Milk");

            // Assert
            Assert.True(actualResult.Count() == 1, errorMessagePrefix + " " + "Filtered recipes should be 1.");
            Assert.True(expectedResult.Id == actualResult.First().Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedLifestyleAndNoAllergies_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndNoAllergies(context);
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
            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix + " " + "Filtered recipes should be 2.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithSpecifiedAllergiesAndNoLifestyle_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForGetAllFilteredAsyncMethodWithSpecifiedAllergiesAndNoLifestyle(context);
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
            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix + " " + "Filtered recipes should be 2.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        [Fact]
        public async Task GetAllFilteredAsync_WithNoSpecifiedLifestyleAndNoAllergies_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService GetAllFilteredAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedDataForGetAllFilteredAsyncMethodWithNoSpecifiedLifestyleAndNoAllergies(context);
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
            Assert.True(actualResult.Count == expectedResult.Count, errorMessagePrefix + " " + "Filtered recipes should be 3.");

            for (int i = 0; i < actualResult.Count; i++)
            {
                Assert.True(expectedResult[i].Id == actualResult[i].Id, errorMessagePrefix + " " + "Recipe is not returned properly.");
            }
        }

        private async Task SeedRecipes(ApplicationDbContext context)
        {
            await context.Categories.AddAsync(new Category() { Title = "Category 1" });
            await context.Categories.AddAsync(new Category() { Title = "Category 2" });
            await context.Categories.AddAsync(new Category() { Title = "Empty category" });
            await context.Users.AddAsync(new ApplicationUser() { Biography = "User with 2 recipes" });
            await context.Users.AddAsync(new ApplicationUser() { Biography = "User with 1 recipe" });
            await context.Users.AddAsync(new ApplicationUser() { Biography = "User with 0 recipes" });
            await context.SaveChangesAsync();

            var recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Title = "Title 1",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Category 1"),
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>
                    {
                        new RecipeAllergen
                        {
                            Allergen = new Allergen() { Name = "AllergenName" },
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = context.Users.First(x => x.Biography == "User with 2 recipes"),
                },
                new Recipe()
                {
                    Title = "Title 2",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Category 1"),
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>
                    {
                        new RecipeAllergen
                        {
                            Allergen = new Allergen() { Name = "AllergenName" },
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = context.Users.First(x => x.Biography == "User with 2 recipes"),
                },
                new Recipe()
                {
                    Title = "Title 3",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Category 2"),
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>
                    {
                        new RecipeAllergen
                        {
                            Allergen = new Allergen() { Name = "AllergenName" },
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = context.Users.First(x => x.Biography == "User with 1 recipe"),
                },
            };

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }

        private async Task SeedRecipe(ApplicationDbContext context)
        {
            var recipe = new Recipe()
            {
                Title = "Title",
                Photo = "Photo",
                Category = new Category() { Title = "CategoryTitle" },
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingList(),
                Allergens = new HashSet<RecipeAllergen>
                {
                    new RecipeAllergen
                    {
                        Allergen = new Allergen() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyle>
                {
                    new RecipeLifestyle
                    {
                        Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                    },
                },
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValue(),
                User = new ApplicationUser(),
            };

            await context.Recipes.AddAsync(recipe);
            await context.SaveChangesAsync();
        }

        private async Task SeedDataForGetAllFilteredAsyncMethodWithNoSpecifiedLifestyleAndNoAllergies(ApplicationDbContext context)
        {
            var user = new ApplicationUser();
            await context.Users.AddAsync(user);
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "Vegetarian" });
            await context.Allergens.AddAsync(new Allergen() { Name = "Milk" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethod(context);
        }

        private async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedAllergiesAndNoLifestyle(ApplicationDbContext context)
        {
            var user = new ApplicationUser();
            user.Allergies.Add(new UserAllergen() { Allergen = new Allergen() { Name = "Milk" } });
            await context.Users.AddAsync(user);
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "Vegetarian" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethod(context);
        }

        private async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndNoAllergies(ApplicationDbContext context)
        {
            var user = new ApplicationUser() { Lifestyle = new Lifestyle() { Type = "Vegetarian" }, };
            await context.Users.AddAsync(user);
            await context.Allergens.AddAsync(new Allergen() { Name = "Milk" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethod(context);
        }

        private async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndAllergies(ApplicationDbContext context)
        {
            var user = new ApplicationUser() { Lifestyle = new Lifestyle() { Type = "Vegetarian" }, };
            user.Allergies.Add(new UserAllergen() { Allergen = new Allergen() { Name = "Milk" } });
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethod(context);
        }

        private async Task SeedRecipesForGetAllFilteredAsyncMethod(ApplicationDbContext context)
        {
            var recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Title = "Vegetarian Recipe without Milk",
                    Photo = "Photo",
                    Category = new Category() { Title = "Category"},
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>()
                    {
                        new RecipeAllergen
                        {
                            Allergen = new Allergen() { Name = "AllergenName" },
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>()
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = context.Lifestyles.First(x => x.Type == "Vegetarian"),
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = new ApplicationUser(),
                },
                new Recipe()
                {
                    Title = "Vegetarian Recipe with Milk",
                    Photo = "Photo",
                    Category = new Category() { Title = "Category" },
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>()
                    {
                        new RecipeAllergen
                        {
                            Allergen = context.Allergens.First(x => x.Name == "Milk"),
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>()
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = context.Lifestyles.First(x => x.Type == "Vegetarian"),
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = new ApplicationUser(),
                },
                new Recipe()
                {
                    Title = "Not Vegetarian Recipe without Milk",
                    Photo = "Photo",
                    Category = new Category() { Title = "Category" },
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>()
                    {
                        new RecipeAllergen
                        {
                            Allergen = new Allergen() { Name = "AllergenName" },
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>()
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = new ApplicationUser(),
                },
            };

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }

        private async Task SeedDataForEditAsyncMethod(ApplicationDbContext context)
        {
            var recipe = new Recipe()
            {
                Title = "Title",
                Photo = "Photo",
                Category = new Category() { Title = "CategoryTitle" },
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingList(),
                Allergens = new HashSet<RecipeAllergen>
                {
                    new RecipeAllergen
                    {
                        Allergen = new Allergen() { Name = "AllergenName" },
                    },
                },
                Lifestyles = new HashSet<RecipeLifestyle>
                {
                    new RecipeLifestyle
                    {
                        Lifestyle = new Lifestyle() { Type = "LifestyleType" },
                    },
                },
                SkillLevel = Level.Easy,
                PreparationTime = 10,
                CookingTime = 10,
                NeededTime = Period.ALaMinute,
                Serving = Size.One,
                NutritionalValue = new NutritionalValue(),
                User = new ApplicationUser(),
            };

            await context.Recipes.AddAsync(recipe);
            await context.Categories.AddAsync(new Category() { Title = "Edited CategoryTitle" });
            await context.Allergens.AddAsync(new Allergen() { Name = "Edited AllergenName" });
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "Edited LifestyleType" });
            await context.SaveChangesAsync();
        }

        private async Task SeedDataForCreateAsyncMethod(ApplicationDbContext context)
        {
            await context.Users.AddAsync(new ApplicationUser());
            await context.Categories.AddAsync(new Category() { Title = "CategoryTitle" });
            await context.Allergens.AddAsync(new Allergen() { Name = "AllergenName" });
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "LifestyleType" });
            await context.SaveChangesAsync();
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
                stringFormatServiceMock.Object);

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

            // LifestyleService
            var lifestyleServiceMock = new Mock<ILifestyleService>();
            lifestyleServiceMock
              .Setup(x => x.SetLifestyleToRecipeAsync(It.IsAny<string>(), It.IsAny<Recipe>()))
              .Callback((string lifestyleType, Recipe recipe) =>
              {
                  recipe.Lifestyles.Add(new RecipeLifestyle() { Lifestyle = context.Lifestyles.First(x => x.Type == lifestyleType) });
              })
              .Returns(Task.FromResult(0));

            // RecipeLifestyleService
            var recipeLifestyleServiceMock = new Mock<IRecipeLifestyleService>();
            recipeLifestyleServiceMock
                .Setup(x => x.DeletePreviousRecipeLifestylesByRecipeId(It.IsAny<string>()))
                .Callback((string recipeId) =>
                {
                    context.RecipeLifestyles.RemoveRange(context.RecipeLifestyles.Where(x => x.RecipeId == recipeId));
                })
                .Verifiable();

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

            // RecipeAllergenService
            var recipeAllergenServiceMock = new Mock<IRecipeAllergenService>();
            recipeAllergenServiceMock
                .Setup(x => x.DeletePreviousRecipeAllergensByRecipeId(It.IsAny<string>()))
                .Callback((string recipeId) =>
                {
                    context.RecipeAllergens.RemoveRange(context.RecipeAllergens.Where(x => x.RecipeId == recipeId));
                })
                .Verifiable();

            // UserFavoriteRecipeService
            var userFavoriteRecipeServiceMock = new Mock<IUserFavoriteRecipeService>();

            // UserCookedRecipeService
            var userCookedRecipeServiceMock = new Mock<IUserCookedRecipeService>();

            // UserAllergenService
            var userAllergenServiceMock = new Mock<IUserAllergenService>();

            // StringFormatService
            var stringFormatServiceMock = new Mock<IStringFormatService>();

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
                stringFormatServiceMock.Object);

            return recipeService;
        }
    }
}
