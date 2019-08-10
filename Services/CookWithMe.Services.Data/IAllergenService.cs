namespace CookWithMe.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CookWithMe.Data.Models;

    public interface IAllergenService
    {
        Task<bool> CreateAllAsync(string[] allergenNames);

        Task<IEnumerable<string>> GetAllNamesAsync();

        Task SetAllergenToRecipeAsync(string allergenName, Recipe recipe);

        Task SetAllergenToUserAsync(string allergenName, ApplicationUser user);

        Task<IEnumerable<int>> GetIdsByNamesAsync(IEnumerable<string> allergenNames);
    }
}
