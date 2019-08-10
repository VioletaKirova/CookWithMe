namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeAllergenService
    {
        Task<ICollection<RecipeAllergenServiceModel>> GetByRecipeIdAsync(string recipeId);

        void DeletePreviousRecipeAllergensByRecipeId(string recipeId);

        Task<ICollection<string>> GetRecipeIdsByAllergenIdsAsync(IEnumerable<int> allergenIds);
    }
}
