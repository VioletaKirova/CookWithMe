namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CookWithMe.Services.Models;

    public interface IRecipeService
    {
        Task<bool> CreateAsync(RecipeServiceModel model);

        Task<IEnumerable<RecipeServiceModel>> GetAllFiltered(string userId);
    }
}
