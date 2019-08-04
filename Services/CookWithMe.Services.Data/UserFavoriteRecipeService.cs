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

        public async Task<bool> ContainsByUserIdAndRecipeId(string userId, string recipeId)
        {
            var userFavoriteRecipesIds = await this.userFavoriteRecipeRepository
               .AllAsNoTracking()
               .Where(x => x.UserId == userId)
               .Select(fr => fr.RecipeId)
               .ToListAsync();

            return userFavoriteRecipesIds.Contains(recipeId);
        }

        public async Task<bool> Remove(string userId, string recipeId)
        {
            var userFavoriteRecipe = await this.userFavoriteRecipeRepository
                .All()
                .SingleOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);

            this.userFavoriteRecipeRepository.Delete(userFavoriteRecipe);

            var result = await this.userFavoriteRecipeRepository.SaveChangesAsync();

            return result > 0;
        }
    }
}
