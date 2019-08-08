namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeLifestyleService
    {
        Task<ICollection<RecipeLifestyleServiceModel>> GetByRecipeId(string recipeId);

        void DeletePreviousRecipeLifestylesByRecipeId(string recipeId);

        Task<ICollection<string>> GetAllRecipeIdsByLifestyleId(int lifestyleId);
    }
}
