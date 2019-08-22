namespace CookWithMe.Services.Data.Tests.Common.Seeders
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data;
    using CookWithMe.Data.Models;
    using CookWithMe.Data.Models.Enums;

    public class RecipeServiceTestsSeeder
    {
        public async Task SeedRecipeAsync(ApplicationDbContext context)
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

        public async Task SeedRecipesAsync(ApplicationDbContext context)
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

        public async Task SeedDataForEditAsyncMethodAsync(ApplicationDbContext context)
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

        public async Task SeedDataForCreateAsyncMethodAsync(ApplicationDbContext context)
        {
            await context.Users.AddAsync(new ApplicationUser());
            await context.Categories.AddAsync(new Category() { Title = "CategoryTitle" });
            await context.Allergens.AddAsync(new Allergen() { Name = "AllergenName" });
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "LifestyleType" });
            await context.SaveChangesAsync();
        }

        public async Task SeedDataForGetAllFilteredAsyncMethodWithNoSpecifiedLifestyleAndNoAllergiesAsync(ApplicationDbContext context)
        {
            var user = new ApplicationUser();
            await context.Users.AddAsync(user);
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "Vegetarian" });
            await context.Allergens.AddAsync(new Allergen() { Name = "Milk" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethodAsync(context);
        }

        public async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedAllergiesAndNoLifestyleAsync(ApplicationDbContext context)
        {
            var user = new ApplicationUser();
            user.Allergies.Add(new UserAllergen() { Allergen = new Allergen() { Name = "Milk" } });
            await context.Users.AddAsync(user);
            await context.Lifestyles.AddAsync(new Lifestyle() { Type = "Vegetarian" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethodAsync(context);
        }

        public async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndNoAllergiesAsync(ApplicationDbContext context)
        {
            var user = new ApplicationUser() { Lifestyle = new Lifestyle() { Type = "Vegetarian" }, };
            await context.Users.AddAsync(user);
            await context.Allergens.AddAsync(new Allergen() { Name = "Milk" });
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethodAsync(context);
        }

        public async Task SeedDataForGetAllFilteredAsyncMethodWithSpecifiedLifestyleAndAllergiesAsync(ApplicationDbContext context)
        {
            var user = new ApplicationUser() { Lifestyle = new Lifestyle() { Type = "Vegetarian" }, };
            user.Allergies.Add(new UserAllergen() { Allergen = new Allergen() { Name = "Milk" } });
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            await this.SeedRecipesForGetAllFilteredAsyncMethodAsync(context);
        }

        public async Task SeedRecipesForGetAllFilteredAsyncMethodAsync(ApplicationDbContext context)
        {
            var recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Title = "Vegetarian Recipe without Milk",
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

        public async Task SeedDataForGetBySearchValuesAsyncMethodAsync(ApplicationDbContext context)
        {
            var categories = new List<Category>()
            {
                new Category() { Title = "Breakfast" },
                new Category() { Title = "Desserts" },
            };
            await context.Categories.AddRangeAsync(categories);

            var allergens = new List<Allergen>()
            {
                new Allergen() { Name = "Milk" },
                new Allergen() { Name = "Peanuts" },
            };
            await context.Allergens.AddRangeAsync(allergens);

            var lifestyles = new List<Lifestyle>()
            {
                new Lifestyle() { Type = "Vegan" },
                new Lifestyle() { Type = "Vegetarian" },
            };
            await context.Lifestyles.AddRangeAsync(lifestyles);

            await context.SaveChangesAsync();

            var recipes = new List<Recipe>()
            {
                new Recipe()
                {
                    Title = "Vegan breakfast with peanuts",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Breakfast"),
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>()
                    {
                        new RecipeAllergen
                        {
                            Allergen = context.Allergens.First(x => x.Name == "Peanuts"),
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>()
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = context.Lifestyles.First(x => x.Type == "Vegan"),
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue()
                    {
                        Calories = 300,
                        Fats = 10,
                    },
                    Yield = 12,
                    User = new ApplicationUser(),
                },
                new Recipe()
                {
                    Title = "Vegetarian breakfast with milk",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Breakfast"),
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
                    SkillLevel = Level.Medium,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.HalfAnHour,
                    Serving = Size.One,
                    NutritionalValue = new NutritionalValue(),
                    User = new ApplicationUser(),
                },
                new Recipe()
                {
                    Title = "Vegan dessert with peanuts",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Desserts"),
                    Summary = "Summary",
                    Directions = "Directions",
                    ShoppingList = new ShoppingList(),
                    Allergens = new HashSet<RecipeAllergen>()
                    {
                        new RecipeAllergen
                        {
                            Allergen = context.Allergens.First(x => x.Name == "Peanuts"),
                        },
                    },
                    Lifestyles = new HashSet<RecipeLifestyle>()
                    {
                        new RecipeLifestyle
                        {
                            Lifestyle = context.Lifestyles.First(x => x.Type == "Vegan"),
                        },
                    },
                    SkillLevel = Level.Easy,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.ALaMinute,
                    Serving = Size.Two,
                    NutritionalValue = new NutritionalValue()
                    {
                        Calories = 300,
                    },
                    Yield = 12,
                    User = new ApplicationUser(),
                },
                new Recipe()
                {
                    Title = "Vegetarian dessert with milk",
                    Photo = "Photo",
                    Category = context.Categories.First(x => x.Title == "Desserts"),
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
                    SkillLevel = Level.Medium,
                    PreparationTime = 10,
                    CookingTime = 10,
                    NeededTime = Period.HalfAnHour,
                    Serving = Size.Two,
                    NutritionalValue = new NutritionalValue(),
                    Yield = 12,
                    User = new ApplicationUser(),
                },
            };

            await context.Recipes.AddRangeAsync(recipes);
            await context.SaveChangesAsync();
        }
    }
}
