namespace CookWithMe.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class UserFavoriteRecipeService : IUserFavoriteRecipeService
    {
        private readonly IRepository<UserFavoriteRecipe> userFavoriteRecipeRepository;

        public UserFavoriteRecipeService(IRepository<UserFavoriteRecipe> userFavoriteRecipeRepository)
        {
            this.userFavoriteRecipeRepository = userFavoriteRecipeRepository;
        }

        public async Task<bool> ContainsByUserIdAndRecipeIdAsync(string userId, string recipeId)
        {
            var userFavoriteRecipesIds = await this.userFavoriteRecipeRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(fr => fr.RecipeId)
               .ToListAsync();

            return userFavoriteRecipesIds.Contains(recipeId);
        }

        public async Task<bool> DeleteByRecipeIdAsync(string recipeId)
        {
            var userFavoriteRecipes = this.userFavoriteRecipeRepository
                .All()
                .Where(x => x.RecipeId == recipeId);

            if (userFavoriteRecipes.Any())
            {
                foreach (var userFavoriteRecipe in userFavoriteRecipes)
                {
                    this.userFavoriteRecipeRepository.Delete(userFavoriteRecipe);
                }
            }

            var result = await this.userFavoriteRecipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteByUserIdAndRecipeIdAsync(string userId, string recipeId)
        {
            var userFavoriteRecipe = await this.userFavoriteRecipeRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);

            this.userFavoriteRecipeRepository.Delete(userFavoriteRecipe);

            var result = await this.userFavoriteRecipeRepository.SaveChangesAsync();

            return result > 0;
        }

        public IQueryable<string> GetRecipeIdsByUserId(string userId)
        {
            return this.userFavoriteRecipeRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.RecipeId);
        }
    }
}
