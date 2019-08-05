namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class RecipeLifestyleService : IRecipeLifestyleService
    {
        private readonly IRepository<RecipeLifestyle> recipeLifestyleRepository;

        public RecipeLifestyleService(IRepository<RecipeLifestyle> recipeLifestyleRepository)
        {
            this.recipeLifestyleRepository = recipeLifestyleRepository;
        }

        public void DeletePreviousLifestylesByRecipeId(string recipeId)
        {
            var recipeLifestyles = this.recipeLifestyleRepository.All().Where(x => x.RecipeId == recipeId);

            foreach (var recipeLifestyle in recipeLifestyles)
            {
                this.recipeLifestyleRepository.Delete(recipeLifestyle);
            }
        }

        public async Task<List<RecipeLifestyleServiceModel>> GetByRecipeId(string recipeId)
        {
            return await this.recipeLifestyleRepository
                .AllAsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .To<RecipeLifestyleServiceModel>()
                .ToListAsync();
        }
    }
}
