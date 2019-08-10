namespace CookWithMe.Services.Data.NutritionalValues
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models.NutritionalValues;

    public interface INutritionalValueService
    {
        Task<string> GetIdByRecipeIdAsync(string recipeId);

        Task<NutritionalValueServiceModel> GetByIdAsync(string id);

        Task EditAsync(string id, NutritionalValueServiceModel nutritionalValueServiceModel);

        Task<bool> DeleteByIdAsync(string id);
    }
}
