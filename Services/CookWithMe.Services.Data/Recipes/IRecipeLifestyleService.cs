namespace CookWithMe.Services.Data.Recipes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.Recipes;

    public interface IRecipeLifestyleService
    {
        Task<ICollection<RecipeLifestyleServiceModel>> GetByRecipeIdAsync(string recipeId);

        void DeletePreviousRecipeLifestylesByRecipeId(string recipeId);

        Task<ICollection<string>> GetRecipeIdsByLifestyleIdAsync(int lifestyleId);
    }
}
