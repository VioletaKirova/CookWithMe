namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeAllergenService
    {
        Task<ICollection<RecipeAllergenServiceModel>> GetByRecipeId(string recipeId);

        void DeletePreviousRecipeAllergensByRecipeId(string recipeId);

        Task<ICollection<string>> GetAllRecipeIdsByAllergenIds(IEnumerable<int> allergenIds);
    }
}
