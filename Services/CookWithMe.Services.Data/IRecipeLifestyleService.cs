namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeLifestyleService
    {
        Task<List<RecipeLifestyleServiceModel>> GetByRecipeId(string recipeId);

        void DeletePreviousLifestylesByRecipeId(string recipeId);
    }
}
