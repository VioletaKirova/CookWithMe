namespace CookWithMe.Services.Data
{
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeService
    {
        Task<bool> CreateAsync(RecipeServiceModel model);
    }
}
