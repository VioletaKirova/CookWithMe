namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface INutritionalValueService
    {
        Task<string> GetIdByRecipeId(string recipeId);

        Task<NutritionalValueServiceModel> GetById(string id);

        Task Edit(string id, NutritionalValueServiceModel model);

        Task<bool> DeleteByRecipeId(string recipeId);
    }
}
