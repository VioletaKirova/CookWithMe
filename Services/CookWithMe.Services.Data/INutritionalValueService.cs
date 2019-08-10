namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface INutritionalValueService
    {
        Task<string> GetIdByRecipeIdAsync(string recipeId);

        Task<NutritionalValueServiceModel> GetByIdAsync(string id);

        Task EditAsync(string id, NutritionalValueServiceModel nutritionalValueServiceModel);

        Task<bool> DeleteByIdAsync(string id);
    }
}
