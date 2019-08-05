namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeAllergenService
    {
        Task<List<RecipeAllergenServiceModel>> GetByRecipeId(string recipeId);

        void DeletePreviousAllergensByRecipeId(string recipeId);
    }
}
