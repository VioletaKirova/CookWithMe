using CookWithMe.Data;
using CookWithMe.Data.Models;
using CookWithMe.Data.Models.Enums;
using CookWithMe.Data.Repositories;
using CookWithMe.Services.Mapping;
using CookWithMe.Services.Data.Allergens;
using CookWithMe.Services.Data.Categories;
using CookWithMe.Services.Data.Lifestyles;
using CookWithMe.Services.Data.NutritionalValues;
using CookWithMe.Services.Data.Recipes;
using CookWithMe.Services.Data.ShoppingLists;
using CookWithMe.Services.Data.Tests.Common;
using CookWithMe.Services.Data.Users;
using CookWithMe.Services.Models.Categories;
using CookWithMe.Services.Models.NutritionalValues;
using CookWithMe.Services.Models.Recipes;
using CookWithMe.Services.Models.ShoppingLists;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CookWithMe.Services.Data.Tests
{
    public class RecipeServiceTests
    {
        [Fact]
        public async Task CreateAsync_WithCorrectData_ShouldReturnCorrectResult()
        {
            string errorMessagePrefix = "RecipeService CreateAsync() method does not work properly.";

            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository);
            var recipeServiceModel = new RecipeServiceModel
            {
                CreatedOn = DateTime.UtcNow,
                Title = "Title",
                Photo = "Photo",
                CategoryId = context.Categories.First().Id,
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
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
            await this.SeedData(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository);
            var recipeServiceModel = new RecipeServiceModel
            {
                CreatedOn = DateTime.UtcNow,
                Title = "Title",
                Photo = "Photo",
                CategoryId = context.Categories.First().Id,
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
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
            var expectedShoppingListId = context.ShoppingLists.First().Id;
            var expecteNutritionalValueId = context.NutritionalValues.First().Id;

            // Assert
            Assert.True(expectedResult.Title == actualResult.Title, errorMessagePrefix + " " + "Title is not returned properly.");
            Assert.True(expectedResult.Photo == actualResult.Photo, errorMessagePrefix + " " + "Photo is not returned properly.");
            Assert.True(expectedResult.CategoryId == actualResult.CategoryId, errorMessagePrefix + " " + "CategoryId is not returned properly.");
            Assert.True(expectedResult.Category.Title == actualResult.Category.Title, errorMessagePrefix + " " + "CategoryId is not returned properly.");
            Assert.True(expectedResult.Summary == actualResult.Summary, errorMessagePrefix + " " + "Summary is not returned properly.");
            Assert.True(expectedResult.Directions == actualResult.Directions, errorMessagePrefix + " " + "Directions are not returned properly.");
            Assert.True(expectedResult.SkillLevel == actualResult.SkillLevel, errorMessagePrefix + " " + "SkillLevel is not returned properly.");
            Assert.True(expectedResult.PreparationTime == actualResult.PreparationTime, errorMessagePrefix + " " + "PreparationTime is not returned properly.");
            Assert.True(expectedResult.CookingTime == actualResult.CookingTime, errorMessagePrefix + " " + "CookingTime is not returned properly.");
            Assert.True(expectedResult.NeededTime == actualResult.NeededTime, errorMessagePrefix + " " + "NeededTime is not returned properly.");
            Assert.True(expectedResult.Serving == actualResult.Serving, errorMessagePrefix + " " + "Serving is not returned properly.");
            Assert.True(expectedResult.UserId == actualResult.UserId, errorMessagePrefix + " " + "UserId is not returned properly.");

            Assert.True(expectedShoppingListId == actualResult.ShoppingListId, errorMessagePrefix + " " + "ShoppingListId is not returned properly.");
            Assert.True(expecteNutritionalValueId == actualResult.NutritionalValueId, errorMessagePrefix + " " + "NutritionalValueId is not returned properly.");
        }

        [Fact]
        public async Task CreateAsync_WithIncorrectData_ShouldThrowArgumentNullException()
        {
            // Arrange
            MapperInitializer.InitializeMapper();
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            await this.SeedData(context);
            var recipeRepository = new EfDeletableEntityRepository<Recipe>(context);
            var recipeService = this.GetRecipeService(recipeRepository);
            var recipeServiceModel = new RecipeServiceModel
            {
                CreatedOn = DateTime.UtcNow,
                Title = null,
                Photo = "Photo",
                CategoryId = context.Categories.First().Id,
                Category = context.Categories.First().To<CategoryServiceModel>(),
                Summary = "Summary",
                Directions = "Directions",
                ShoppingList = new ShoppingListServiceModel(),
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

        private async Task SeedData(ApplicationDbContext context)
        {
            await context.Users.AddAsync(new ApplicationUser());
            await context.Categories.AddAsync(new Category() { Title = "Title" });
            await context.SaveChangesAsync();
        }

        private RecipeService GetRecipeService(EfDeletableEntityRepository<Recipe> recipeRepository)
        {
            var categoryServiceMock = new Mock<ICategoryService>();
            var lifestyleServiceMock = new Mock<ILifestyleService>();
            var recipeLifestyleServiceMock = new Mock<IRecipeLifestyleService>();
            var userServiceMock = new Mock<IUserService>();
            var shoppingListServiceMock = new Mock<IShoppingListService>();
            var userShoppingListServiceMock = new Mock<IUserShoppingListService>();
            var nutritionalValueServiceMock = new Mock<INutritionalValueService>();
            var allergenServiceMock = new Mock<IAllergenService>();
            var recipeAllergenServiceMock = new Mock<IRecipeAllergenService>();
            var userFavoriteRecipeServiceMock = new Mock<IUserFavoriteRecipeService>();
            var userCookedRecipeServiceMock = new Mock<IUserCookedRecipeService>();
            var userAllergenServiceMock = new Mock<IUserAllergenService>();
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
