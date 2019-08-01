namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models;

    using Microsoft.EntityFrameworkCore;

    public class NutritionalValueService : INutritionalValueService
    {
        private readonly IDeletableEntityRepository<NutritionalValue> nutritionalValueRepository;

        public NutritionalValueService(IDeletableEntityRepository<NutritionalValue> nutritionalValueRepository)
        {
            this.nutritionalValueRepository = nutritionalValueRepository;
        }

        public async Task<NutritionalValueServiceModel> GetById(string id)
        {
            var nutritionalValue = await this.nutritionalValueRepository
                .GetByIdWithDeletedAsync(id);

            return nutritionalValue.To<NutritionalValueServiceModel>();
        }

        public async Task<string> GetIdByRecipeId(string recipeId)
        {
            var nutritionalValue = await this.nutritionalValueRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == recipeId);

            return nutritionalValue.Id;
        }
    }
}
