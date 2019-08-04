﻿namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class UserCookedRecipeService : IUserCookedRecipeService
    {
        private readonly IRepository<UserCookedRecipe> userCookedRecipeRepository;

        public UserCookedRecipeService(IRepository<UserCookedRecipe> userCookedRecipeRepository)
        {
            this.userCookedRecipeRepository = userCookedRecipeRepository;
        }

        public async Task<bool> ContainsByUserIdAndRecipeId(string userId, string recipeId)
        {
            var userCookedRecipesIds = await this.userCookedRecipeRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(cr => cr.RecipeId)
               .ToListAsync();

            return userCookedRecipesIds.Contains(recipeId);
        }

        public async Task<bool> Remove(string userId, string recipeId)
        {
            var userCookedRecipe = await this.userCookedRecipeRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);

            this.userCookedRecipeRepository.Delete(userCookedRecipe);

            var result = await this.userCookedRecipeRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
