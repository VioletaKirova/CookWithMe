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

    public class RecipeAllergenService : IRecipeAllergenService
    {
        private readonly IRepository<RecipeAllergen> recipeAllergenRepository;

        public RecipeAllergenService(IRepository<RecipeAllergen> recipeAllergenRepository)
        {
            this.recipeAllergenRepository = recipeAllergenRepository;
        }

        public void DeletePreviousAllergensByRecipeId(string recipeId)
        {
            var recipeAllergens = this.recipeAllergenRepository.All().Where(x => x.RecipeId == recipeId);

            foreach (var recipeAllergen in recipeAllergens)
            {
                this.recipeAllergenRepository.Delete(recipeAllergen);
            }
        }

        public async Task<ICollection<string>> GetAllRecipeIdsByAllergenIds(IEnumerable<int> allergenIds)
        {
            return await this.recipeAllergenRepository
                .AllAsNoTracking()
                .Where(x => allergenIds.Contains(x.AllergenId))
                .Select(x => x.RecipeId)
                .ToListAsync();
        }

        public async Task<List<RecipeAllergenServiceModel>> GetByRecipeId(string recipeId)
        {
            return await this.recipeAllergenRepository
                .AllAsNoTracking()
                .Where(x => x.RecipeId == recipeId)
                .To<RecipeAllergenServiceModel>()
                .ToListAsync();
        }
    }
}
