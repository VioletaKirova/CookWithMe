namespace CookWithMe.Services.Data.NutritionalValues
{
    using System.Threading.Tasks;

    using CookWithMe.Data.Common.Repositories;
    using CookWithMe.Data.Models;
    using CookWithMe.Services.Mapping;
    using CookWithMe.Services.Models.NutritionalValues;

    using Microsoft.EntityFrameworkCore;

    public class NutritionalValueService : INutritionalValueService
    {
        private readonly IDeletableEntityRepository<NutritionalValue> nutritionalValueRepository;

        public NutritionalValueService(IDeletableEntityRepository<NutritionalValue> nutritionalValueRepository)
        {
            this.nutritionalValueRepository = nutritionalValueRepository;
        }

        public async Task<bool> DeleteByIdAsync(string id)
        {
            var nutritionalValue = await this.nutritionalValueRepository
                .GetByIdWithDeletedAsync(id);

            this.nutritionalValueRepository.Delete(nutritionalValue);
            var result = await this.nutritionalValueRepository.SaveChangesAsync();

            return result > 0;
        }

        public async Task EditAsync(string id, NutritionalValueServiceModel nutritionalValueServiceModel)
        {
            var nutritionalValueFromDb = await this.nutritionalValueRepository
                .GetByIdWithDeletedAsync(id);

            nutritionalValueFromDb.Calories = nutritionalValueServiceModel.Calories;
            nutritionalValueFromDb.Fats = nutritionalValueServiceModel.Fats;
            nutritionalValueFromDb.SaturatedFats = nutritionalValueServiceModel.SaturatedFats;
            nutritionalValueFromDb.Carbohydrates = nutritionalValueServiceModel.Carbohydrates;
            nutritionalValueFromDb.Sugar = nutritionalValueServiceModel.Sugar;
            nutritionalValueFromDb.Protein = nutritionalValueServiceModel.Protein;
            nutritionalValueFromDb.Fiber = nutritionalValueServiceModel.Fiber;
            nutritionalValueFromDb.Salt = nutritionalValueServiceModel.Salt;

            this.nutritionalValueRepository.Update(nutritionalValueFromDb);
        }

        public async Task<NutritionalValueServiceModel> GetByIdAsync(string id)
        {
            var nutritionalValue = await this.nutritionalValueRepository
                .GetByIdWithDeletedAsync(id);

            return nutritionalValue.To<NutritionalValueServiceModel>();
        }

        public async Task<string> GetIdByRecipeIdAsync(string recipeId)
        {
            var nutritionalValue = await this.nutritionalValueRepository
                .AllAsNoTracking()
                .SingleOrDefaultAsync(x => x.RecipeId == recipeId);

            return nutritionalValue.Id;
        }
    }
}
