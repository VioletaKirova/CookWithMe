namespace CookWithMe.Services.Data.Users
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.Recipes;

    using Microsoft.EntityFrameworkCore;

    public class UserCookedRecipeService : IUserCookedRecipeService
    {
        private const string InvalidUserCookedRecipeErrorMessage = "UserCookedRecipe with UserId: {0} and RecipeId: {1} does not exist.";

        private readonly IRepository<UserCookedRecipe> userCookedRecipeRepository;

        public UserCookedRecipeService(IRepository<UserCookedRecipe> userCookedRecipeRepository)
        {
            this.userCookedRecipeRepository = userCookedRecipeRepository;
        }

        public async Task<bool> ContainsByUserIdAndRecipeIdAsync(string userId, string recipeId)
        {
            var userCookedRecipesIds = await this.userCookedRecipeRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(cr => cr.RecipeId)
               .ToListAsync();

            return userCookedRecipesIds.Contains(recipeId);
        }

        public async Task<bool> DeleteByRecipeIdAsync(string recipeId)
        {
            var userCookedRecipes = this.userCookedRecipeRepository
                .All()
                .Where(x => x.RecipeId == recipeId);

            if (userCookedRecipes.Any())
            {
                foreach (var userCookedRecipe in userCookedRecipes)
                {
                    this.userCookedRecipeRepository.Delete(userCookedRecipe);
                }
            }

            var result = await this.userCookedRecipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByUserIdAndRecipeIdAsync(string userId, string recipeId)
        {
            var userCookedRecipe = await this.userCookedRecipeRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);

            if (userCookedRecipe == null)
            {
                throw new ArgumentNullException(
                    string.Format(InvalidUserCookedRecipeErrorMessage, userId, recipeId));
            }

            this.userCookedRecipeRepository.Delete(userCookedRecipe);

            var result = await this.userCookedRecipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<RecipeServiceModel> GetRecipesByUserId(string userId)
        {
            return this.userCookedRecipeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedOn)
                .Select(x => x.Recipe)
                .To<RecipeServiceModel>();
        }
    }
}
