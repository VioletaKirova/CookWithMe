namespace CookWithMe.Services.Data.Lifestyles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;
    using CookWithMe.Services.Models.Lifestyles;

    public interface ILifestyleService
    {
        Task<bool> CreateAllAsync(string[] types);

        Task<IEnumerable<string>> GetAllTypesAsync();

        Task SetLifestyleToRecipeAsync(string lifestyleType, Recipe recipe);

        Task SetLifestyleToUserAsync(string lifestyleType, ApplicationUser user);

        Task<LifestyleServiceModel> GetByIdAsync(int id);

        Task<int> GetIdByTypeAsync(string lifestyleType);
    }
}
